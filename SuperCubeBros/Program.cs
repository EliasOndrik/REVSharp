using Silk.NET.Windowing;
using Silk.NET.Maths;
using SuperCubeBros;

internal class Program
{
    private static void Main()
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(1920, 1080),
            Title = "Super Cube Bros",
            WindowState = WindowState.Maximized
        };
        SuperCube game = new(options);
        game.Run();
    }
}