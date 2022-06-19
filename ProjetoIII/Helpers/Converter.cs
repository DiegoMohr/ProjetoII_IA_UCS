namespace ProjetoIII.Helpers;

public static class Converter
{
    public static string ConvertCoordinate(int row, int column)
        => $"{(char)(column + 65)}{row + 1}";

    public static string ConvertCoordinate((int, int) coordinate)
    {
        var (row, column) = coordinate;
        return ConvertCoordinate(row, column);
    }

    public static (int, int)? ConvertCoordinate(string coordinate)
    {
        if (coordinate.Length < 2)
            return null;

        coordinate = coordinate.ToUpper();
        var col = coordinate[0] - 65;

        coordinate = coordinate.Substring(1, coordinate.Length - 1);
        if (!int.TryParse(coordinate, out var row))
            return null;

        return (row - 1, col);
    }
}