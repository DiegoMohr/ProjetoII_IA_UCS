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
}

public static class Program
{
    public static void Main(string[] args)
    {
        var mapMatrix = LoadMap();
        var states = LoadStates(mapMatrix);

    }

    private static int[,] LoadMap()
    {
        using var reader = new StreamReader("D:\\ia.txt");
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

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var actions = new List<Action>();
                var number = map[i, j];

                if (i - 1 >= 0 && map[i - 1, j] != -1)
                    actions.Add(new Action { Index = (i - 1, j) });
                if (i + 1 < rows && map[i + 1, j] != -1)
                    actions.Add(new Action { Index = (i + 1, j) });
                if (j - 1 >= 0 && map[i, j - 1] != -1)
                    actions.Add(new Action { Index = (i, j - 1) });
                if (j + 1 < cols && map[i, j + 1] != -1)
                    actions.Add(new Action { Index = (i, j + 1) });

                states[i, j] = new State
                {
                    Name = $"S{i * rows + j + 1}",
                    IsTerminal = number == 100,
                    Active = number != -1,
                    Actions = actions
                };
            }
        }

        return states;
    }
}