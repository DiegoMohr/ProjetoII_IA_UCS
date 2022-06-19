namespace ProjetoIII.Models;

public class Action
{
    public (int, int) Index { get; set; }
    public string Name { get; set; }
    public double Reward { get; set; }
    public State State { get; set; }
}