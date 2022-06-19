using System.Diagnostics;
using ProjetoIII.Helpers;
using ProjetoIII.Models;

namespace ProjetoIII.UI;

public static class Cli
{
    private static readonly HistoryText[] _history = new HistoryText[3];
    private static readonly IDictionary<string, System.Action> _methods = LoadMethods();
    private static bool _exit;
    private static IDictionary<string, string[]> _options;
    private static string _lastCommand;
    private static Runtime _runtime;

    public static void Run()
    {
        Console.CancelKeyPress += (_, args) =>
        {
            if (_runtime?.Simulating == true)
            {
                _runtime.CancelRequested = true;
                args.Cancel = true;
            }
            else
            {
                Console.Clear();
                Process.GetCurrentProcess().Kill();
            }
        };

        while (!_exit)
        {
            if (!TryGetInput(out var input))
                continue;

            CliGraphics.ClearErrorMessage();

            if (TryParseCommand(input, out var method))
            {
                DisplayHistory();
                method.Invoke();
            }
            else
            {
                CliGraphics.DrawErrorMessage($"{input} is not recognized. Type help if you need to see the commands.");
                _history[0].Error = true;
                DisplayHistory();
            }
        }
    }

    private static bool TryParseCommand(string input, out System.Action method)
    {
        method = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        var split = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = split.First().ToLower();
        if (!_methods.ContainsKey(command))
            return false;

        _lastCommand = command;
        method = _methods[command];

        var options = input.Split('-').Skip(1).ToList();

        _options = new Dictionary<string, string[]>(options.Count);

        foreach (var option in options)
        {
            split = option.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _options.Add(split[0], split.Skip(1).ToArray());
        }

        return true;
    }

    public static bool TryGetInput(out string input)
    {
        DisplayHistory();
        CliGraphics.DrawCliInput();

        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return false;

        _history[2] = _history[1];
        _history[1] = _history[0];
        _history[0] = new HistoryText(input);

        CliGraphics.DrawCliInput();
        return true;
    }

    private static void DisplayHistory()
    {
        CliGraphics.DrawCliHistory(_history);
    }

    private static IDictionary<string, System.Action> LoadMethods()
        => new Dictionary<string, System.Action>
        {
            {"load", LoadMap},
            {"q", Exit},
            {"quit", Exit},
            {"exit", Exit},
            {"reset", Reset},
            {"model", SetOptions},
            {"train", TrainModel},
            {"run", RunModel},
            {"table", ShowFullTable},
            {"help", ShowHelp},
            {"h", ShowHelp},
            {"stats", ShowStats}
        };

    private static void ShowStats()
    {
        if (!CheckRuntime())
            return;

        CliGraphics.DrawHeader("Statistics");
        _runtime.DisplayStats();
        CliGraphics.DrawPressAnyKey();
        PressAnyKey();

        CliGraphics.DrawHeader();
        _runtime.DrawRuntime();
    }

    private static void ShowHelp()
    {
        CliGraphics.DrawHeader("Help");
        CliGraphics.ClearMiddleScreen();
        CliGraphics.DrawHelpText();
        PressAnyKey();

        if (_runtime != null)
        {
            _runtime.DrawRuntime();
            return;
        }

        CliGraphics.DrawHeader();
        CliGraphics.DrawIntro();
    }

    public static void PressAnyKey()
    {
        CliGraphics.DrawPressAnyKey();
        Console.ReadKey();
    }

    private static void SetStartPosition(string[] args, ref string errors)
    {
        if (args.Length != 1)
        {
            errors += "Please specify a value for Start Position | ";
            return;
        }

        var pos = Converter.ConvertCoordinate(args[0]);
        if (pos == null)
        {
            errors += $"Invalid coordinate {args[1]} | ";
            return;
        }

        _runtime.Options.StartPos = pos.Value;
    }

    private static void SetGama(string[] args, ref string errors)
    {
        if (args.Length != 1 || !double.TryParse(args[0], out var value) || value is <= 0 or > 1)
        {
            errors += "Gama must be > 0 and < 1 | ";
            return;
        }

        _runtime.Options.Gama = value;
    }

    private static void SetBestChoices(string[] args, ref string errors)
    {
        if (args.Length != 1 || !double.TryParse(args[0], out var value) || value is < 0 or > 100)
        {
            errors += "Choices must be >= 0 and <= 100 | ";
            return;
        }

        _runtime.Options.BestChoice = value;
    }

    private static void SetOptions()
    {
        if (!CheckRuntime() || !ValidateOptionsLength(1, 3))
            return;

        var errors = string.Empty;

        foreach (var (option, args) in _options)
        {
            switch (option)
            {
                case "start":
                case "s":
                    SetStartPosition(args, ref errors);
                    break;
                case "gama":
                case "g":
                    SetGama(args, ref errors);
                    break;
                case "choices":
                case "c":
                    SetBestChoices(args, ref errors);
                    break;
                case "default":
                case "d":
                    _runtime.Options.ResetToDefault();
                    break;
                default:
                    errors += $"{option} is not recognized. | ";
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(errors))
            CliGraphics.DrawErrorMessage(errors.Remove(errors.Length - 2));
    }

    private static void LoadMap()
    {
        var path = string.Empty;
        var name = "ia_project";

        if (!ValidateOptionsLength(0, 1))
            return;

        if (_options.Count == 0)
            path = "maps\\map_final.txt";
        else if (_options.Count == 1)
        {
            var (option, args) = _options.First();
            if (!ValidateOption(option, 1, 1, "m", "map"))
                return;

            name = args[0];
            path = $"maps\\{name}.txt";
        }

        if (!Validate(File.Exists(path), $"Map {name} not found."))
            return;

        CliGraphics.DrawHeader("Runtime");
        _runtime = new Runtime();
        _runtime.Load(path, name);
    }

    private static void Exit()
    {
        Console.Clear();
        _exit = true;
    }

    private static void Reset()
    {
        _runtime = null;
        CliGraphics.DrawHeader();
        CliGraphics.DrawIntro();
    }

    private static bool Validate(bool condition, string message)
    {
        if (!condition)
            CliGraphics.DrawErrorMessage(message);
        return condition;
    }

    private static void TrainModel()
    {
        if (!CheckRuntime() || !ValidateOptionsLength(0, 3))
            return;

        foreach (var (option, _) in _options)
        {
            if (!ValidateOption(option, 0, 0, "d", "debug", "r", "restart", "s", "stats"))
                return;

            if (option is "d" or "debug")
            {
                _runtime.DebugMode = true;
                CliGraphics.DrawDebugOutline();
            }
            else if (option is "r" or "restart")
            {
                _runtime.Restart = true;
            }
            else if (option is "s" or "stats")
                _runtime.ShowStatsAfterTraining = true;
        }

        if (_runtime.Options.Trained && !_runtime.Restart)
        {
            CliGraphics.DrawErrorMessage("Model already trained! Use -r to restart the model");
            return;
        }

        _runtime.Train();
    }

    private static bool ValidateOptionsLength(int min, int max)
    {
        if (_options.Count >= min && _options.Count <= max)
            return true;

        CliGraphics.DrawErrorMessage($"{_lastCommand} expects between {min} and {max} options");
        return false;
    }

    private static bool ValidateOption(string option, int minArgs, int maxArgs, params string[] expectedOptions)
    {
        if (expectedOptions.All(opt => opt != option))
        {
            CliGraphics.DrawErrorMessage($"-{option} is not recognized");
            return false;
        }

        var args = _options[option];
        if (args.Length >= minArgs && args.Length <= maxArgs)
            return true;

        var argText = maxArgs > 1 ? "arguments" : "argument";
        var text = minArgs == maxArgs ? $"expects {maxArgs} {argText}" : $"expects between {minArgs} and {maxArgs} {argText}";
        CliGraphics.DrawErrorMessage($"-{option} {text}");
        return false;
    }

    private static void RunModel()
    {
        if (!CheckRuntime() || !ValidateOptionsLength(0, 2))
            return;

        (int, int)? coordinate = null;
        var forceRun = false;

        foreach (var (option, args) in _options)
        {
            if (option is "s" or "start")
            {
                if (!ValidateOption(option, 1, 1, "s", "start"))
                    return;

                coordinate = Converter.ConvertCoordinate(args[0]);
                if (!Validate(coordinate != null && _runtime.ValidPosition(coordinate.Value),
                        $"Invalid coordinate {args[0]}"))
                    return;
            }
            else if (option is "f" or "force")
                forceRun = true;
        }

        if (!forceRun && !Validate(_runtime.Options.Trained, "Model is not trained yet"))
            return;

        _runtime.Run(coordinate);
    }

    private static void ShowFullTable()
    {
        if (!CheckRuntime())
            return;

        CliGraphics.DrawHeader("Full Q-Table");
        _runtime.DisplayFullTable();
        _runtime.DrawRuntime();
        DisplayHistory();
    }

    private static bool CheckRuntime()
    {
        if (_runtime != null)
            return true;

        CliGraphics.DrawErrorMessage("Runtime not started. Load a map first!");
        return false;
    }

    public static ConsoleKey GetKeyInput(params ConsoleKey[] expected)
    {
        while (true)
        {
            var keyInfo = Console.ReadKey(true);
            if (expected.Any(key => key == keyInfo.Key))
                return keyInfo.Key;
        }
    }

    public static void DisplayCli()
    {
        CliGraphics.DrawCliInput();
        CliGraphics.DrawCliHistory(_history);
    }
}