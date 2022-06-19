using ProjetoIII.Helpers;
using ProjetoIII.UI;
using System.Diagnostics;
using ProjetoIII.Models;
using Action = ProjetoIII.Models.Action;

namespace ProjetoIII;

public class Runtime
{
    private int[,] _map;
    private State[,] _states;
    private IList<Action> _actions;
    private readonly IList<Statistic> _statistics = new List<Statistic>();

    private int _mapRows;
    private int _mapCols;

    public RuntimeOptions Options { get; } = new();

    public bool CancelRequested { get; set; }

    public void Load(string path, string name)
    {
        CliGraphics.ClearMiddleScreen();
        CliGraphics.DrawRuntimeLayout();

        LoadMap(path);
        LoadStates();
        LoadTable();

        CliGraphics.DrawMap(_map);
        CliGraphics.DrawActionsTable(_actions, null);
        CliGraphics.DrawStatus(237, "Ready");
        Options.MapName = name;
    }

    public void DisplayFullTable()
    {
        var lines = CliGraphics.Height - 14;
        var columns = Math.Ceiling(_actions.Count / (double)lines);
        var columnsPerPage = Math.Floor(CliGraphics.Width / 25.0);
        var pages = Math.Ceiling(columns / columnsPerPage);

        CliGraphics.DrawKeyboardInputHelp(new List<string> {"F9", "F10", "ESC"}, 
                                          new List<string> { "Previous Page", "Next Page", "Return"});
        var currentPage = 0;
        var back = false;

        while (!back)
        {
            CliGraphics.DrawActionsTablePage(_actions, (int) columnsPerPage, lines, (int) pages, currentPage);

            var input = Cli.GetKeyInput(ConsoleKey.F9, ConsoleKey.F10, ConsoleKey.Escape);
            switch (input)
            {
                case ConsoleKey.F9 when currentPage > 0:
                    currentPage--;
                    break;
                case ConsoleKey.F10 when currentPage < pages - 1:
                    currentPage++;
                    break;
                case ConsoleKey.Escape:
                    back = true;
                    break;
            }
        }
    }

    public void DrawRuntime()
    {
        CliGraphics.ClearMiddleScreen();
        CliGraphics.DrawRuntimeLayout();
        CliGraphics.DrawMap(_map);
        CliGraphics.DrawActionsTable(_actions, null);
        CliGraphics.DrawOptions(Options);
        CliGraphics.DrawStatus(237, "Ready");
    }

    private void LoadMap(string path)
    {
        using var reader = new StreamReader(path);
        var content = reader.ReadToEnd();
        var lines = content.Split('\n');

        var first = lines.First();

        _mapRows = lines.Length;
        _mapCols = first.Split(' ').Length;

        _map = new int[_mapRows, _mapCols];

        for (int i = 0; i < _mapRows; i++)
        {
            var split = lines[i].Split(' ');
            for (int j = 0; j < _mapCols; j++)
                _map[i, j] = int.Parse(split[j]);
        }
    }

    private void LoadStates()
    {
        _states = new State[_mapRows, _mapCols];
        var actionsCount = 1;

        for (int i = 0; i < _mapRows; i++)
        {
            for (int j = 0; j < _mapCols; j++)
            {
                var number = _map[i, j];

                var state = new State
                {
                    Name = Converter.ConvertCoordinate(i, j),
                    IsTerminal = number is 100,
                    IsObstacle = number is -100,
                    Active = number is not -1,
                    Reward = number == 1 ? 0 : number
                };

                var actions = new List<Action>();

                if (i - 1 >= 0 && _map[i - 1, j] != -1)
                    actions.Add(new Action { Index = (i - 1, j), Name = $"a{actionsCount++}", State = state });
                if (i + 1 < _mapRows && _map[i + 1, j] != -1)
                    actions.Add(new Action { Index = (i + 1, j), Name = $"a{actionsCount++}", State = state });
                if (j - 1 >= 0 && _map[i, j - 1] != -1)
                    actions.Add(new Action { Index = (i, j - 1), Name = $"a{actionsCount++}", State = state });
                if (j + 1 < _mapCols && _map[i, j + 1] != -1)
                    actions.Add(new Action { Index = (i, j + 1), Name = $"a{actionsCount++}", State = state });

                state.Actions = actions;
                _states[i, j] = state;
            }
        }
    }

    private void LoadTable()
    {
        var actionsTable = new List<Action>();

        for (int i = 0; i < _states.GetLength(0); i++)
        {
            for (int j = 0; j < _states.GetLength(1); j++)
            {
                var state = _states[i, j];
                actionsTable.AddRange(state.Actions);
            }
        }

        _actions = actionsTable;
    }

    public bool DebugMode { get; set; }
    public bool Simulating { get; private set; }
    public bool Restart { get; set; }
    public bool ShowStatsAfterTraining { get; set; }

    private void RestartModel()
    {
        foreach (var action in _actions)
            action.Reward = 0;
    }

    private void GetDebugInput()
    {
        var key = Cli.GetKeyInput(ConsoleKey.F10, ConsoleKey.Spacebar);
        switch (key)
        {
            case ConsoleKey.F10:
                break;
            case ConsoleKey.Spacebar:
                DebugMode = false;
                CliGraphics.ClearDebugOutline();
                Cli.DisplayCli();
                break;
        }
    }

    public void Train()
    {
        var position = ValidPosition(Options.StartPos) ? Options.StartPos : GetRandomPosition();

        CliGraphics.HideCursor();
        Simulating = true;
        DrawRuntime();
        CliGraphics.DrawStatus(208, "Training...");

        if (Restart)
            RestartModel();

        if (DebugMode)
        {
            CliGraphics.DrawDebugOutline();
            CliGraphics.DrawKeyboardInputHelp(new List<string>{ "F10", "Space" }, new List<string> { "Step", "Exit Debug" });
        }
        
        var gama = Options.Gama;
        var bestChoices = Options.BestChoice;

        var iterations = 0;

        var totalActionsToTrain = _actions.Count(act => act.State.Active && act.State.Reward == 0);
        var actionsTrained = 0;

        var watch = Stopwatch.StartNew();

        while (actionsTrained < totalActionsToTrain && !CancelRequested)
        {
            CliGraphics.DrawPlayer(position);
            var (i, j) = position;
            var currentState = _states[i, j];
            Action currentAction;
            bool bestChoice;

            var actions = currentState.Actions;

            if (actions.All(act => act.Reward == 0))
            {
                currentAction = actions[Rng.NextInt(0, actions.Count)];
                position = currentAction.Index;
                bestChoice = true;
            }
            else
            {
                if (Rng.CheckPercentage(bestChoices))
                {
                    var ordered = actions.OrderByDescending(act => act.Reward).ToList();
                    var first = ordered.First();
                    var highest = first.Reward;
                    var highestCount = ordered.Count(act => act.Reward == highest);
                    currentAction = highestCount > 1 ? actions[Rng.NextInt(0, highestCount)] : first;
                    position = currentAction.Index;
                    bestChoice = true;
                }
                else
                {
                    var random = Rng.NextInt(0, actions.Count);
                    currentAction = actions[random];
                    position = currentAction.Index;
                    bestChoice = false;
                }
            }
            
            (i, j) = position;

            // atualiza q-learning
            var nextState = _states[i, j];
            var reward = nextState.Reward + gama * MaxRewardOf(nextState.Actions);
            
            if (DebugMode)
            {
                CliGraphics.DrawDebugInfo(currentState, nextState, currentAction, reward, bestChoice);
                GetDebugInput();
            }

            if (currentAction.Reward == 0 && reward != 0)
                actionsTrained++;

            currentAction.Reward = reward;

            if (nextState.IsTerminal || nextState.IsObstacle)
            {
                CliGraphics.DrawPlayer(position);
                position = GetRandomPosition(true);
                iterations++;
                var status = $"Training...     Iterations: {iterations}      Actions Trained: {actionsTrained}/{totalActionsToTrain}          " +
                             $"Time: {watch.Elapsed.Hours:D2}:{watch.Elapsed.Minutes:D2}:{watch.Elapsed.Seconds:D2}";
                CliGraphics.DrawStatus(208, status);
                if (DebugMode)
                {
                    CliGraphics.DrawDebugInfo(nextState, Converter.ConvertCoordinate(position));
                    GetDebugInput();
                }
            }

            CliGraphics.DrawActionsTable(_actions, currentAction);
        }

        watch.Stop();

        if (CancelRequested)
        {
            DrawRuntime();
            CancelRequested = false;
            Simulating = false;
            return;
        }

        Options.Trained = true;

        _statistics.Add(new Statistic(Options.MapName, iterations, Options.Gama, Options.BestChoice, watch.Elapsed));

        if (ShowStatsAfterTraining)
        {
            CliGraphics.DrawStatistics(_statistics);
            Cli.PressAnyKey();
        }

        Restart = false;
        ShowStatsAfterTraining = false;
        Simulating = false;
        DrawRuntime();
        CliGraphics.ShowCursor();
    }

    private (int, int) GetRandomPosition(bool notTrained = false)
    {
        int i, j;
        var tries = 0;

        while (true)
        {
            i = Rng.NextInt(0, _mapRows);
            j = Rng.NextInt(0, _mapCols);
            var state = _states[i, j];
            if (!state.IsTerminal && !state.IsObstacle && state.Active)
            {
                if (notTrained && state.Actions.All(act => act.Reward != 0) && tries < 25)
                {
                    tries++;
                    continue;
                }

                break;
            }
        }

        return (i, j);
    }

    public bool ValidPosition((int, int) position)
    {
        var (row, col) = position;
        if (row < 0 || row >= _mapRows || col < 0 || col >= _mapCols)
            return false;

        var state = _states[row, col];
        return state.Active && !state.IsTerminal && !state.IsObstacle;
    }

    public void Run((int, int)? startPos = null)
    {
        CliGraphics.HideCursor();
        Simulating = true;
        CliGraphics.DrawStatus(39, "Running...");

        CliGraphics.DrawPathOutline();

        var path = new List<string>();
        var finished = false;

        var position = startPos ?? GetRandomPosition();
        CliGraphics.DrawPlayer(position);

        while (!finished)
        {
            Thread.Sleep(750);

            var (i, j) = position;
            var currentState = _states[i, j];
            var actions = _actions.Where(act => act.State == currentState).ToList();

            var bestAction = actions.OrderByDescending(act => act.Reward).First();
            position = bestAction.Index;

            path.Add($"{currentState.Name}({bestAction.Reward:F})");
            CliGraphics.DrawPath(path);

            (i, j) = position;
            var nextState = _states[i, j];

            CliGraphics.DrawPlayer(position);

            if (nextState.IsTerminal || nextState.IsObstacle || CancelRequested)
            {
                if (CancelRequested)
                    DrawRuntime();
                else
                {
                    CliGraphics.DrawPlayer(position);
                    path.Add(nextState.Name);
                    CliGraphics.DrawPath(path);
                }
                finished = true;
            }
        }

        CancelRequested = false;
        Simulating = false;
        CliGraphics.DrawStatus(237, "Ready");
        CliGraphics.ShowCursor();
    }

    private static double MaxRewardOf(IEnumerable<Action> actions)
    {
        var max = 0.0;
        foreach (var action in actions)
            max = Math.Max(max, action.Reward);

        return max;
    }

    public void DisplayStats()
    {
        CliGraphics.DrawStatistics(_statistics);
    }
}