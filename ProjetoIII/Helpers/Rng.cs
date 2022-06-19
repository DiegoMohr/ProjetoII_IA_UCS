namespace ProjetoIII.Helpers;

public static class Rng
{
    private static readonly Random _random = new(Guid.NewGuid().GetHashCode());

    public static int NextInt(int max) => _random.Next(max);
    public static int NextInt(int min, int max) => _random.Next(min, max);

    public static bool CheckPercentage(double percentage)
    {
        var checkValue = percentage / 10;
        var number = _random.NextDouble() * 10;
        return number <= checkValue;
    }
}