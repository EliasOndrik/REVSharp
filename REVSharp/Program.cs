using REVSharp;
using REVSharp.Core;
using Silk.NET.Maths;

internal class Program
{
    private static void Main(string[] args)
    {
        ECS ecs = new();

        Entity entity = new();
        Entity entity1 = new();
        Entity entity2 = new();

        ecs.RegisterComponent<Transform>();
        Move move = new();
        ecs.RegisterSystem(move);
        ecs.SetMask<Move>(ecs.GetMask<Transform>());

        ecs.AddComponent<Transform>(entity, new());
        ecs.AddComponent<Transform>(entity2, new());

        Console.WriteLine(ecs.GetComponent<Transform>(entity).Position);
        Console.WriteLine(ecs.GetComponent<Transform>(entity2).Position);
        Console.WriteLine(entity.Id);
        move.Update(0.0f);
        move.Update(0.0f);
        move.Update(0.0f);
        Console.WriteLine(ecs.GetComponent<Transform>(entity).Position);
        Console.WriteLine(ecs.GetComponent<Transform>(entity2).Position);
        ecs.RemoveEntity(entity);
        Console.WriteLine(entity.Id);


    }
}