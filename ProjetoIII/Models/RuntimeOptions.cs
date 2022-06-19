using ProjetoIII.UI;

namespace ProjetoIII.Models;

public class RuntimeOptions
{
    private (int, int) _startPos = (0, 0);
    private bool _trained;
    private string _mapName;
    private double _gama = 0.5;
    private double _bestChoice = 85;

    public bool Trained
    {
        get => _trained;
        set
        {
            _trained = value;
            CliGraphics.DrawOptions(this);
        }
    }

    public (int, int) StartPos
    {
        get => _startPos;
        set
        {
            _startPos = value;
            CliGraphics.DrawPlayer(value);
            CliGraphics.DrawOptions(this);
        }
    }

    public string MapName
    {
        get => _mapName;
        set
        {
            _mapName = value;
            CliGraphics.DrawOptions(this);
        }
    }

    public double Gama
    {
        get => _gama;
        set
        {
            _gama = value;
            CliGraphics.DrawOptions(this);
        }
    }

    public double BestChoice
    {
        get => _bestChoice;
        set
        {
            _bestChoice = value;
            CliGraphics.DrawOptions(this);
        }
    }

    public void ResetToDefault()
    {
        _startPos = (0, 0);
        _gama = 0.5;
        _bestChoice = 85;
        CliGraphics.DrawOptions(this);
    }
}