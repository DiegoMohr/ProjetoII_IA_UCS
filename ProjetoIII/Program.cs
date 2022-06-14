namespace ProjetoII;

public struct State
{
    public bool Active { get; set; }
    public string Name { get; set; }
    public bool IsTerminal { get; set; }
    public int Reward { get; set; }
    public List<Action> Actions { get; set; }
}

public struct Action
{
    public (int, int) Index { get; set; }
    public string Name { get; set; }
}

public struct Function
{
    public State State { get; set; }
    public Action Action { get; set; }
    public int Reward { get; set; }
}

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Clear();
        Cli.DrawHeader();
        Cli.DivideScreen();

        var mapMatrix = LoadMap();
        var states = LoadStates(mapMatrix);
        var functions = new List<Function>();

        for (int i = 0; i < states.GetLength(0); i++)
        {
            for (int j = 0; j < states.GetLength(1); j++)
            {
                var state = states[i, j];
                functions.AddRange(state.Actions
                    .Select(act => new Function {State = state, Action = act}));
            }
        }
        
        Cli.DrawMap(mapMatrix);
        Cli.DrawFunctionsTable(functions);
        Console.ReadKey();
    }

    private static int[,] LoadMap()
    {
        using var reader = new StreamReader("maps\\map_final.txt");
        var content = reader.ReadToEnd();
        var lines = content.Split('\n');

        var first = lines.First();

        var rows = lines.Length;
        var cols = first.Split(' ').Length;

        var mapMatrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            var split = lines[i].Split(' ');
            for (int j = 0; j < cols; j++)
                mapMatrix[i, j] = int.Parse(split[j]);
        }

        return mapMatrix;
    }

    private static State[,] LoadStates(int[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);

        var states = new State[rows, cols];
        var actionsCount = 1;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var actions = new List<Action>();
                var number = map[i, j];

                if (i - 1 >= 0 && map[i - 1, j] != -1)
                    actions.Add(new Action { Index = (i - 1, j), Name = $"a{actionsCount++}" });
                if (i + 1 < rows && map[i + 1, j] != -1)
                    actions.Add(new Action { Index = (i + 1, j), Name = $"a{actionsCount++}" });
                if (j - 1 >= 0 && map[i, j - 1] != -1)
                    actions.Add(new Action { Index = (i, j - 1), Name = $"a{actionsCount++}" });
                if (j + 1 < cols && map[i, j + 1] != -1)
                    actions.Add(new Action { Index = (i, j + 1), Name = $"a{actionsCount++}" });

                states[i, j] = new State
                {
                    Name = $"{(char)(j + 65)}{i + 1}",
                    IsTerminal = number == 100,
                    Active = number != -1,
                    Actions = actions,
                    Reward = number
                };
            }
        }

        return states;
    }
}