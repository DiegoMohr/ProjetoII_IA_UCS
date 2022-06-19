namespace ProjetoIII.Models;

public class State
{
    public bool Active { get; set; }
    public string Name { get; set; }
    public bool IsTerminal { get; set; }
    public bool IsObstacle { get; set; }
    public double Reward { get; set; }
    public List<Action> Actions { get; set; }
}