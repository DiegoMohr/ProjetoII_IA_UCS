using System.Text;

namespace ProjetoII;

public static class Cli
{
    public static int Width => Console.WindowWidth;
    public static int Height => Console.WindowHeight;

    private static int _firstWidth;
    private static int _secondWidth;

    public static void DrawHeader()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(GetCentralizedText("Projeto III - Q Learning", Width));
        Console.SetCursorPosition(0, 1);
        Console.Write(GetFilledLine('ˉ', Width));
    }

    public static void DivideScreen()
    {
        _firstWidth = (int) (Width * 0.75);
        _secondWidth = Width - _firstWidth;

        for (int i = 2; i < Height; i++)
        {
            Console.SetCursorPosition(_firstWidth, i);
            Console.Write('│');
        }
    }

    private static string GetCentralizedText(string text, int width)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < width / 2 - text.Length / 2; i++)
            sb.Append(' ');

        if (width % 2 != 0)
            sb.Append(' ');

        var spaces = sb.ToString();

        sb = new StringBuilder();
        sb.Append(spaces);
        sb.Append(text);

        if (text.Length % 2 != 0) 
            spaces = spaces.Remove(spaces.Length - 1, 1);

        sb.Append(spaces);
        return sb.ToString();
    }

    private static string GetFilledLine(char character, int width)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < width; i++)
            sb.Append(character);

        return sb.ToString();
    }

    public static void DrawFunctionsTable(IList<Function> functions)
    {
        var width = _firstWidth + 4;
        var secondWidth = width + 16;
        Console.SetCursorPosition(width, 2);
        Console.Write("+---------+------------+");
        Console.SetCursorPosition(width, 3);
        Console.Write("|   Q()   |   Reward   |");
        Console.SetCursorPosition(width, 4);
        Console.Write("+---------+------------+");

        var x = 5;
        for (int i = 0; i < 20; i++)
        {
            var func = functions[i];
            Console.SetCursorPosition(width, x);
            Console.Write("| ");
            //Console.BackgroundColor = x % 2 == 0 ? ConsoleColor.Magenta : ConsoleColor.Black;
            Console.Write($"{func.State.Name},{func.Action.Name}                ");
            Console.SetCursorPosition(secondWidth, x++);
            Console.Write($"{func.Reward}      ");
            //Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("|");
        }

        Console.SetCursorPosition(width, x);
        Console.Write("+---------+------------+");

        //Console.BackgroundColor = ConsoleColor.Black;
    }

    public static void DrawMap(int[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        Console.SetCursorPosition(0, 3);

        var sb = new StringBuilder();
        sb.Append("  ");

        for (int i = 0; i < cols; i++) 
            sb.Append($"   {(char) (i + 65)}");

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
                    middleBuilder.Append("   ");
                    bottomBuilder.Append("   ");
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
    }
}