using ProjetoIII.Helpers;
using System.Text;
using ProjetoIII.Models;
using Action = ProjetoIII.Models.Action;

namespace ProjetoIII.UI;

public static class CliGraphics
{
    public static int Width => Console.WindowWidth;
    public static int Height => Console.WindowHeight;

    private static (int, int) _lastPosition;
    private static bool _errorShowing;

    public static void Start()
    {
        Console.Clear();
        DrawProgramLayout();
        DrawHeader();
        DrawIntro();
    }

    private static void DrawProgramLayout()
    {
        var line = GetFilledLine('ˉ', Width);
        Console.Write($"\x1b[2;0H{line}");
        Console.Write($"\x1b[{Height - 5};0H{line}");
    }

    public static void ClearMiddleScreen()
    {
        ClearLinesFrom(3, Height - 6);
    }

    public static void DrawIntro()
    {
        ClearMiddleScreen();
        Console.SetCursorPosition(0, 4);

        Console.Write("\x1b[5;38;5;207m");

        Console.WriteLine(GetCentralizedText(" ██████        ██      ███████  █████  ██████  ███    ██ ██ ███    ██  ██████ ", Width - 1));
        Console.WriteLine(GetCentralizedText("██    ██       ██      ██      ██   ██ ██   ██ ████   ██ ██ ████   ██ ██      ", Width - 1));
        Console.WriteLine(GetCentralizedText("██    ██ █████ ██      █████   ███████ ██████  ██ ██  ██ ██ ██ ██  ██ ██   ███", Width - 1));
        Console.WriteLine(GetCentralizedText("██ ▄▄ ██       ██      ██      ██   ██ ██   ██ ██  ██ ██ ██ ██  ██ ██ ██    ██", Width - 1));
        Console.WriteLine(GetCentralizedText(" ██████        ███████ ███████ ██   ██ ██   ██ ██   ████ ██ ██   ████  ██████ ", Width - 1));
        Console.WriteLine(GetCentralizedText("    ▀▀                                                                         ", Width - 1));

        Console.Write("\x1b[0;38;5;51m");

        Console.SetCursorPosition(0, 12);
        Console.WriteLine(GetCentralizedText("██████╗ ██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ", Width - 1));
        Console.WriteLine(GetCentralizedText("██╔══██╗██╔══██╗██║   ██║████╗  ██║██╔═══██╗", Width - 1));
        Console.WriteLine(GetCentralizedText("██████╔╝██████╔╝██║   ██║██╔██╗ ██║██║   ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██╔══██╗██╔══██╗██║   ██║██║╚██╗██║██║   ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██████╔╝██║  ██║╚██████╔╝██║ ╚████║╚██████╔╝", Width - 1));
        Console.WriteLine(GetCentralizedText("╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ", Width - 1));

        Console.Write("\n\n");

        Console.WriteLine(GetCentralizedText(" ██████╗██╗  ██╗██████╗ ██╗███████╗████████╗██╗ █████╗ ███╗   ██╗", Width - 1));
        Console.WriteLine(GetCentralizedText("██╔════╝██║  ██║██╔══██╗██║██╔════╝╚══██╔══╝██║██╔══██╗████╗  ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██║     ███████║██████╔╝██║███████╗   ██║   ██║███████║██╔██╗ ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██║     ██╔══██║██╔══██╗██║╚════██║   ██║   ██║██╔══██║██║╚██╗██║", Width - 1));
        Console.WriteLine(GetCentralizedText("╚██████╗██║  ██║██║  ██║██║███████║   ██║   ██║██║  ██║██║ ╚████║", Width - 1));
        Console.WriteLine(GetCentralizedText(" ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚══════╝   ╚═╝   ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝", Width - 1));

        Console.Write("\n\n");

        Console.WriteLine(GetCentralizedText("██████╗ ██╗███████╗ ██████╗  ██████╗ ", Width - 1));
        Console.WriteLine(GetCentralizedText("██╔══██╗██║██╔════╝██╔════╝ ██╔═══██╗", Width - 1));
        Console.WriteLine(GetCentralizedText("██║  ██║██║█████╗  ██║  ███╗██║   ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██║  ██║██║██╔══╝  ██║   ██║██║   ██║", Width - 1));
        Console.WriteLine(GetCentralizedText("██████╔╝██║███████╗╚██████╔╝╚██████╔╝", Width - 1));
        Console.WriteLine(GetCentralizedText("╚═════╝ ╚═╝╚══════╝ ╚═════╝  ╚═════╝ ", Width - 1));


        Console.SetCursorPosition(0, Height - 7);
        Console.Write("\x1b[5;38;5;255m");
        Console.Write($"{GetCentralizedText("=-- Welcome to our demo! Type help to see the commands --=", Width - 1)}");
        Console.Write("\x1b[0m");
    }

    public static void DrawHeader(string header = null)
    {
        Console.Write("\x1b[H\x1b[K");

        var text = "Projeto III - Q Learning";
        if (header != null)
            text += $" | {header}";

        Console.Write(GetCentralizedText(text, Width));
    }

    private static string GetCentralizedText(string text, int width)
    {
        if (width <= text.Length)
            return text;

        var sb = new StringBuilder();

        for (int i = 0; i < width / 2 - text.Length / 2; i++)
            sb.Append(' ');

        if (width % 2 != 0)
            sb.Append(' ');

        var spaces = sb.ToString();

        sb = new StringBuilder();
        sb.Append(spaces);
        sb.Append(text);
        sb.Append(spaces);

        var result = sb.ToString();
        if (result.Length == width)
            return result;

        if (result.Length - width == 2)
            result = result.Remove(0, 1);

        return result.Remove(result.Length - 1);
    }

    private static string GetFilledLine(char character, int width)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < width; i++)
            sb.Append(character);

        return sb.ToString();
    }

    private static int _tableRows;

    public static void DrawRuntimeLayout()
    {
        _tableRows = Height - 22;

        Console.SetCursorPosition(0, 3);
        Console.Write(GetFilledLine('-', Width));

        var left = Width - 26;
        Console.SetCursorPosition(left, 5);
        Console.Write("+-----------+------------+");
        Console.SetCursorPosition(left, 6);
        Console.Write("|    Q()    |   Reward   |");
        Console.SetCursorPosition(left, 7);
        Console.Write("+-----------+------------+");

        for (int i = 8; i < _tableRows + 8; i++)
        {
            Console.SetCursorPosition(left, i);
            Console.Write("|                        |");
        }

        Console.SetCursorPosition(left, _tableRows + 8);
        Console.Write("+-----------+------------+");
    }

    public static void DrawActionsTablePage(IList<Action> actions, int columns, int lines, int pages, int currentPage)
    {
        ClearMiddleScreen();

        var offset = lines * columns * currentPage;
        for (int i = 0; i < columns; i++)
        {
            var acts = actions.Take(new Range(lines * i + offset, lines * (i + 1) + offset)).ToList();
            if (acts.Count == 0)
                break;

            DrawActionsTableColumn(acts, i * 25, lines);
        }

        Console.SetCursorPosition(0, Height - 7);
        var pageText = $"{currentPage + 1}";
        if (currentPage != 0)
            pageText = pageText.Insert(0, "<< ");
        if (currentPage + 1 < pages)
            pageText = pageText.Insert(pageText.Length, " >>");

        Console.Write(GetCentralizedText($"Page: {pageText}", Width));
    }

    public static void DrawDebugOutline()
    {
        DrawBox(Width - 56, 6, 30, Height - 18, "Debug");
    }

    public static void ClearDebugOutline()
    {
        ClearBox(Width - 56, 6, 30, Height - 18);
    }

    private static void DrawBox(int x, int y, int width, int height, string name = null)
    {
        Console.Write($"\x1b[{y};{x}H");

        var text = "+---";
        if (name != null)
            text += $" {name} ";

        text += $"{GetFilledLine('-', width - text.Length - 1)}+";
        Console.Write(text);

        for (int i = y + 1; i < y + height - 1; i++)
        {
            Console.Write($"\x1b[{i};{x}H");
            Console.Write($"|{GetFilledLine(' ', width - 2)}|");
        }

        Console.Write($"\x1b[{y + height - 1};{x}H");
        Console.Write($"+{GetFilledLine('-', width - 2)}+");
    }

    private static void ClearBox(int x, int y, int width, int height)
    {
        for (int i = y; i < y + height; i++)
        {
            Console.Write($"\x1b[{i};{x}H");
            Console.Write($"{GetFilledLine(' ', width)}");
        }
    }

    public static void DrawPressAnyKey()
    {
        ClearLinesFrom(Height - 4);
        Console.Write($"\x1b[{Height - 4};0H");
        Console.Write("\x1b[3;5mPress any key to continue...\x1b[0m");
    }

    private static void ClearLinesFrom(int from, int to)
    {
        var diff = to - from;

        if (diff < 0)
            return;

        var command = $"\x1b[{from};0H\x1b[K";
        Console.Write(command);
        for (int i = 0; i < diff; i++)
        {
            command += "\x1b[1B\x1b[K";
            Console.Write(command);
        }

        Console.Write(command);
    }

    private static void ClearLinesFrom(int from)
    {
        ClearLinesFrom(from, Height);
    }

    private static void DrawActionsTableColumn(IList<Action> actions, int left, int lines)
    {
        Console.SetCursorPosition(left, 3);
        Console.Write("+-----------+------------+");
        Console.SetCursorPosition(left, 4);
        Console.Write("|    Q()    |   Reward   |");
        Console.SetCursorPosition(left, 5);
        Console.Write("+-----------+------------+");

        for (int i = 0; i < actions.Count; i++)
        {
            var act = actions[i];
            Console.SetCursorPosition(left, i + 6);
            Console.Write($"| {GetTextWithFilledSpaces($"{act.State.Name},{act.Name}", 12)}{GetTextWithFilledSpaces($"{act.Reward:F3}", 11)}|");
        }

        if (actions.Count != lines)
        {
            var remaining = lines - actions.Count;
            var lastIndex = Height - 9 - remaining;

            for (int i = Height - 9; i > lastIndex; i--)
            {
                Console.SetCursorPosition(left, i);
                Console.Write("|                        |");
            }
        }

        Console.SetCursorPosition(left, Height - 8);
        Console.Write("+-----------+------------+");
    }

    public static void DrawKeyboardInputHelp(IList<string> keys, IList<string> descriptions)
    {
        ClearLinesFrom(Height - 4);

        for (int i = 0; i < keys.Count; i++)
        {
            Console.Write($"\x1b[{Height - 4};{i * 26}H");
            Console.Write($"\x1b[7m{GetCentralizedText($"{keys[i]}", 8)}\x1b[27m {descriptions[i]}");
        }

        Console.CursorVisible = false;
    }

    public static void DrawActionsTable(IList<Action> actions, Action currentAction)
    {
        var left = Width - 24;
        var index = actions.IndexOf(currentAction);

        var halfRows = _tableRows / 2;

        var finish = index + halfRows;
        var start = finish >= actions.Count ? actions.Count - _tableRows : index - halfRows - 1;

        if (start < 0)
            start = 0;

        var y = 8;
        for (int i = 0; i < _tableRows; i++)
        {
            index = i + start;
            if (index >= actions.Count)
                break;

            var action = actions[index];
            Console.SetCursorPosition(left, y++);
            if (action == currentAction)
                Console.BackgroundColor = ConsoleColor.DarkBlue;

            Console.Write(GetTextWithFilledSpaces($"{action.State.Name},{action.Name}", 12));
            Console.Write(GetTextWithFilledSpaces($"{action.Reward:F3}", 11));
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    public static void DrawMap(int[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        Console.SetCursorPosition(0, 5);

        var sb = new StringBuilder();
        sb.Append("  ");

        for (int i = 0; i < cols; i++)
            sb.Append($"   {(char)(i + 65)}");

        Console.WriteLine(sb.ToString());

        for (int i = 0; i < rows; i++)
        {
            var middleBuilder = new StringBuilder();
            var bottomBuilder = new StringBuilder();

            middleBuilder.Append(i >= 9 ? $"{i + 1} " : $" {i + 1} ");
            bottomBuilder.Append("   ");

            for (int j = 0; j < cols; j++)
            {
                var state = map[i, j];
                if (state == -1)
                {
                    middleBuilder.Append("    ");
                    bottomBuilder.Append("    ");
                    continue;
                }

                for (int k = 0; k < 4; k++)
                {
                    if (k == 0)
                    {
                        middleBuilder.Append('|');
                        bottomBuilder.Append('+');
                    }
                    else
                    {
                        middleBuilder.Append(' ');
                        bottomBuilder.Append('-');
                    }
                }

                if (j + 1 < cols && map[i, j + 1] == -1 || j + 1 == cols)
                {
                    middleBuilder.Append('|');
                    bottomBuilder.Append('+');
                }
            }

            var middle = middleBuilder.ToString();
            var bottom = bottomBuilder.ToString();

            if (i == 0)
                Console.WriteLine(bottom);

            Console.WriteLine(middle);
            Console.WriteLine(bottom);
        }

        ColorMap(map);
    }

    private static string GetTextWithFilledSpaces(string text, int width)
    {
        var sb = new StringBuilder();
        sb.Append(text);

        for (int i = 0; i < width - text.Length; i++)
            sb.Append(' ');

        return sb.ToString();
    }

    public static void DrawPlayer((int, int) position)
    {
        RemovePlayer();

        var (row, col) = position;
        Console.Write($"\x1b[{8 + 2 * row};{6 + 4 * col}H");
        Console.Write("\x1b[38;5;201m■\x1b[0m");
        _lastPosition = position;
    }

    public static void RemovePlayer()
    {
        var (row, col) = _lastPosition;
        Console.Write($"\x1b[{8 + 2 * row};{6 + 4 * col}H");
        Console.Write(' ');
    }

    private static readonly Timer _errorTimer = new(ClearError, null, TimeSpan.FromDays(25), TimeSpan.FromDays(25));

    public static void DrawErrorMessage(string message)
    {
        Console.Write($"\x1b[{Height - 3};0H\x1b[48;5;124m{GetTextWithFilledSpaces(message, Width)}\x1b[0m");
        _errorTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromDays(25));
        _errorShowing = true;
    }

    private static void ClearError(object _)
    {
        ClearErrorMessage();
    }

    public static void ClearErrorMessage()
    {
        if (!_errorShowing)
            return;

        Console.Write("\x1b[s");
        Console.Write($"\x1b[{Height - 3};0H\x1b[K");
        Console.Write("\x1b[u");
        _errorTimer.Change(TimeSpan.FromDays(25), TimeSpan.FromDays(25));
        _errorShowing = false;
    }

    private static void ColorMap(int[,] map)
    {
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                var state = map[row, col];
                if (state == 100)
                {
                    Console.SetCursorPosition(5 + 4 * col - 1, 7 + 2 * row);
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write("   ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (state == -100)
                {
                    Console.SetCursorPosition(5 + 4 * col - 1, 7 + 2 * row);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write("   ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }
    }

    public static void DrawCliInput()
    {
        Console.Write($"\x1b[{Height - 4};0H\x1b[K> ");
        Console.CursorVisible = true;
    }

    public static void DrawOptions(RuntimeOptions options)
    {
        Console.SetCursorPosition(0, 2);

        var optionsWidth = Width / 5;

        var line = GetTextWithFilledSpaces($"Map: {options.MapName}", optionsWidth);

        var text = GetTextWithFilledSpaces($"Trained: {options.Trained}", optionsWidth);
        var color = options.Trained ? 40 : 160;
        text = text.Insert(8, $"\x1b[38;5;{color}m").Insert(text.Length, "\x1b[38;5;255m");
        line += text;

        text = GetTextWithFilledSpaces($"Gama: {options.Gama}", optionsWidth);
        color = options.Gama == 0.5 ? 255 : 208;
        var style = options.Gama == 0.5 ? 0 : 3;
        text = text.Insert(5, $"\x1b[{style};38;5;{color}m").Insert(text.Length, "\x1b[0;38;5;255m");
        line += text;

        text = GetTextWithFilledSpaces($"Best Choices: {options.BestChoice}%", optionsWidth);
        color = options.BestChoice == 85 ? 255 : 208;
        style = options.BestChoice == 85 ? 0 : 3;
        text = text.Insert(13, $"\x1b[{style};38;5;{color}m").Insert(text.Length, "\x1b[0;38;5;255m");
        line += text;

        text = GetTextWithFilledSpaces($"Start State: {Converter.ConvertCoordinate(options.StartPos)}", optionsWidth);
        color = options.StartPos == (0, 0) ? 255 : 208;
        style = options.StartPos == (0, 0) ? 0 : 3;
        text = text.Insert(12, $"\x1b[{style};38;5;{color}m").Insert(text.Length, "\x1b[0;38;5;255m");
        line += text;

        Console.Write(line);
    }

    public static void DrawDebugInfo(State currentState, State nextState, Action currentAction, double reward, bool bestChoice)
    {
        ClearBox(Width - 55, 7, 28, Height - 20);

        var y = 7;
        int color;
        Console.Write($"\x1b[{y++};{Width - 54}H");
        Console.Write($"Current State: {currentState.Name}");
        foreach (var action in currentState.Actions)
        {
            Console.Write($"\x1b[{y++};{Width - 55}H");
            color = action == currentAction ? 7 : 0;
            Console.Write($"\x1b[{color}m{GetTextWithFilledSpaces($"    {action.Name} -> {Converter.ConvertCoordinate(action.Index)} ({action.Reward:F3})", 28)}\x1b[0m");
        }

        y++;
        Console.Write($"\x1b[{y++};{Width - 54}H");
        color = bestChoice ? 40 : 124;
        Console.Write($"Best Choice: \x1b[38;5;{color}m{bestChoice}\x1b[0m");

        Console.Write($"\x1b[{y++};{Width - 54}H");
        color = currentAction.Reward > reward ? 124 : currentAction.Reward < reward ? 40 : 255;
        Console.Write($"Calc Reward: \x1b[38;5;{color}m{reward:F3}\x1b[0m");

        Console.Write($"\x1b[{y++};{Width - 54}H");
        color = nextState.IsTerminal ? 40 : nextState.IsObstacle ? 124 : 255;
        Console.Write($"Next State: \x1b[38;5;{color}m{nextState.Name}\x1b[0m");
    }

    public static void DrawDebugInfo(State currentState, string nextCoordinate)
    {
        var y = 7;
        ClearBox(Width - 55, y, 28, Height - 20);

        Console.Write($"\x1b[{y++};{Width - 55}H");
        var color = currentState.IsTerminal ? 40 : 124;
        var status = currentState.IsTerminal ? "Success" : "Fail";
        Console.Write($"\x1b[48;5;{color}m{GetCentralizedText($"--= {status} =--", 28)}\x1b[0m");
        Console.Write($"\x1b[{y++};{Width - 54}H");
        Console.Write($"Current State: \x1b[38;5;{color}m{currentState.Name}\x1b[0m");
        Console.Write($"\x1b[{y++};{Width - 54}H");
        Console.Write($"Next State: {nextCoordinate}");
    }

    public static void DrawStatus(int color, string status = "")
    {
        Console.SetCursorPosition(0, Height - 7);
        Console.Write($"\x1b[3;48;5;{color}m");
        Console.Write(GetTextWithFilledSpaces(status, Width));
        Console.Write("\x1b[0;48;5;0m");
    }

    public static void DrawStatistics(IList<Statistic> statistics)
    {
        ClearMiddleScreen();

        var y = 4;

        Console.Write($"\x1b[{y++};0H");
        var line = GetFilledLine('-', 17);
        var outline = $"+{line}+{line}+{line}+{line}+{line}+";
        Console.Write(outline);
        Console.Write($"\x1b[{y++};0H");
        Console.Write($"|{GetCentralizedText("Map", 17)}|{GetCentralizedText("Iterations", 17)}|{GetCentralizedText("Gama", 17)}|{GetCentralizedText("Best Choices", 17)}|{GetCentralizedText("Time", 17)}|");
        Console.Write($"\x1b[{y++};0H");
        Console.Write(outline);

        line = $"|{GetFilledLine(' ', 17 * 5 + 4)}|";

        for (; y < Height - 7; y++)
        {
            Console.Write($"\x1b[{y};0H");
            Console.Write(line);
        }

        Console.Write($"\x1b[{y};0H");
        Console.Write(outline);

        y = 7;
        var x = 2;

        foreach (var stat in statistics)
        {
            Console.Write($"\x1b[{y++};{x}H");
            var text = GetTextWithFilledSpaces(stat.MapName, 18);
            text += GetTextWithFilledSpaces($"{stat.Iterations}", 18);
            text += GetTextWithFilledSpaces($"{stat.Gama:F1}", 18);
            text += GetTextWithFilledSpaces($"{stat.BestChoices:F1}%", 18);
            text += GetTextWithFilledSpaces($"{stat.Time:g}", 17);
            Console.Write(text);
        }
    }

    public static void HideCursor()
    {
        Console.CursorVisible = false;
    }

    public static void ShowCursor()
    {
        Console.CursorVisible = true;
    }

    public static void DrawPathOutline()
    {
        DrawBox(0, Height - 12, Width, 5, "Path");
    }

    public static void DrawPath(IList<string> path)
    {
        var top = Height - 12;
        var characters = 0;
        Console.SetCursorPosition(2, top++);

        for (int i = 0; i < path.Count; i++)
        {
            var str = path[i];
            characters += str.Length + 4;

            if (characters > Width - 4)
            {
                characters = 0;
                Console.SetCursorPosition(2, top++);
            }

            if (!str.EndsWith(")"))
            {
                str = str.Insert(0, "\x1b[38;5;40m");
                str = str.Insert(str.Length, "\x1b[0m");
            }

            Console.Write($"{str}");
            if (i != path.Count - 1)
                Console.Write($" -> ");
        }
    }

    public static void ClearPathFull()
    {
        ClearLinesFrom(Height - 12, Height - 8);
    }

    public static void DrawHelpText()
    {
        Console.SetCursorPosition(0, 2);

        Console.Write("\x1b[38;5;246mWelcome to the Help page!\n\x1b[38;5;255mCommands are 'white'\x1b[38;5;246m,\x1b[38;5;219m options are 'light purple'\x1b[38;5;246m and\x1b[38;5;80m arguments are 'light blue'\x1b[38;5;246m\n");
        Console.Write("Usage: \x1b[38;5;255mCommand\x1b[38;5;246m -\x1b[38;5;219moption \x1b[38;5;80marg1 arg2\x1b[38;5;255m\n\n");

        Console.Write("Commands:\n");
        // Load
        Console.Write("\nload\n");
        DrawHelpOption("-m, -map {0}",
                       "Loads a {0} from the maps folder.",
                       "MapName");
        DrawHelpOption("-r, -random {0} {1}",
                       "Loads a random map with {0} rows and {1} columns.",
                       "R", "C");

        // Options
        Console.Write("\nmodel\n");
        DrawHelpDescription("Adjusts the training parameters for the model.");
        DrawHelpOption("-s, -start {0}",
                       "Sets the start position to {0} for the training process. Example: A1",
                       "Pos");
        DrawHelpOption("-g, -gama {0}",
                       "Sets the gama to {0}.",
                       "Value");
        DrawHelpOption("-c, -choices {0}",
                       "Sets the percentage that the training model will choose the best action to {0}.",
                       "Value");
        DrawHelpOption("-d, -default",
                       "Sets all the options to their default values.");

        // Train
        Console.Write("\ntrain");
        DrawHelpDescription("Starts the training process.");
        DrawHelpOption("-d, -debug", "Starts training with debug mode.");
        DrawHelpOption("-r, -restart", "Restarts the model and starts training.");
        DrawHelpOption("-s, -stats", "Shows the stats page after training completes.");

        // Run
        Console.Write("\nrun");
        DrawHelpDescription("Gives a random start location to the agent and simulates how it moves in the map.");
        DrawHelpOption("-s, -start {0}",
                       "Start the simulation on the start location {0}",
                       "Pos");
        DrawHelpOption("-f, -force",
                       "Force start if model is not trained.");

        // Table
        Console.Write("\nstats");
        DrawHelpDescription("Shows the stats page for the current runtime.");

        // Table
        Console.Write("\nreset");
        DrawHelpDescription("Resets the application.");

        // Table
        Console.Write("\ntable");
        DrawHelpDescription("Shows the full Q-Table.");

        // Help
        Console.Write("\nh, help");
        DrawHelpDescription("Displays the help page.");

        // Exit
        Console.Write("\nq, quit, exit");
        DrawHelpDescription("Exits the application.");
    }

    private static void DrawHelpOption(string option, string description, params string[] arguments)
    {
        for (int i = 0; i < arguments.Length; i++)
        {
            var arg = arguments[i];
            option = option.Replace($"{{{i}}}", ToArgument(arg, 219));
            description = description.Replace($"{{{i}}}", ToArgument(arg, 246));
        }

        Console.Write($"\r\x1b[38;5;219m {option}\x1b[0m");
        DrawHelpDescription(description);
    }

    private static string ToArgument(string argument, int resetColor = 255)
        => $"\x1b[38;5;80m{argument}\x1b[38;5;{resetColor}m";

    private static void DrawHelpDescription(string description)
    {
        Console.Write($"\x1b[40G\x1b[38;5;246m{description}\x1b[0m\n");
    }

    public static void DrawCliHistory(HistoryText[] history)
    {
        Console.SetCursorPosition(0, Height - 4);

        for (int i = 0; i < 3; i++)
        {
            var hist = history[i];
            if (hist == null)
                continue;

            var color = hist.Error ? 31 : 96;
            Console.Write($"\r\x1b[1B\x1b[K  \x1b[3;{color}m{hist.Text}");
        }

        Console.Write("\x1b[0m");
    }
}