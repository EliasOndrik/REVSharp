using REVSharp;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using AppleFall;
internal class Program
{
    private static void Main()
    {
        
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Apple Fall";
        AppleFallGame game = new(options);
        game.Run();
    }
}