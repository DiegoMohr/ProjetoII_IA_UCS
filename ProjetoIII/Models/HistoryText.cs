namespace ProjetoIII.Models;

public class HistoryText
{
    public HistoryText(string text)
    {
        Text = text;
    }

    public string Text { get; }
    public bool Error { get; set; }
}