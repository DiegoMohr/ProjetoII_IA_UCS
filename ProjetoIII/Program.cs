using ProjetoIII.UI;

namespace ProjetoIII;

public static class Program
{
    private const int MinWidth = 130;
    private const int MinHeight = 50;

    public static void Main(string[] args)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        ValidateTerminalDimensions();

        CliGraphics.Start();
        Cli.Run();
    }
    
    private static void ValidateTerminalDimensions()
    {
        var validated = false;

        while (!validated)
        {
            if (Console.WindowWidth >= MinWidth && Console.WindowHeight >= MinHeight)
                validated = true;
            else
            {
                Console.Clear();
                Console.WriteLine(Environment.CurrentDirectory);
                Console.WriteLine($"Window size too small for the program to run properly.\nPlease adjust the size to at least width {MinWidth} and height {MinHeight}");
                Console.WriteLine($"Current size: Width: {Console.WindowWidth} Height: {Console.WindowHeight}\n");
                Console.WriteLine("To toggle fullscreen press Alt + Enter.");
                Console.Write("Press any key to continue after adjusting the size...");
                Console.ReadKey();
            }
        }
    }
}