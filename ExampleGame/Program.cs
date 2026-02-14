using ExampleGame;
using REVSharp;
using Silk.NET.Maths;
using Silk.NET.Windowing;

internal class Program
{
    private static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Example Game";
        Game1 game = new(options);
        game.Run();
    }
}