namespace ProjetoIII.Models;

public readonly struct Statistic
{
    public Statistic(string mapName, int iterations, double gama, double bestChoices, TimeSpan time)
    {
        MapName = mapName;
        Iterations = iterations;
        Gama = gama;
        BestChoices = bestChoices;
        Time = time;
    }

    public string MapName { get; }
    public int Iterations { get; }
    public double Gama { get; }
    public double BestChoices { get; }
    public TimeSpan Time { get; }
}