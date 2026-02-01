using REVSharp;
using REVSharp.Components;
using REVSharp.Core;
using Silk.NET.Maths;

namespace ExampleGame
{
    internal class Game1 : Game
    {
        //private Entity Entity;
        private Entity en;
        protected override void Load()
        {
            Ecs.RegisterSystem(new RotateBanana());
            uint mask = Ecs.GetComponentMask<Transform>();
            Ecs.SetSystemMask<RotateBanana>(mask);
            //Entity = Ecs.CreateEntity();
            //Ecs.AddComponent(Entity, new Transform());
            //Ecs.AddComponent(Entity, new Model(_gl, "cube.obj"));
            //Ecs.AddComponent(Entity, new Vertex());
            _modelManager.LoadModel("vaza.dae");
            int modelId = _modelManager.GetModelIndex("vaza.dae");
            for (int i = 0; i < 1000; i++)
            {
                en = Ecs.CreateEntity();
                Ecs.AddComponent(ref en, new Transform
                {
                    Position = new Vector3D<float>(0.0f, -2.0f, 0.0f - i),
                    Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
                    Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
                });
                Ecs.AddComponent(ref en, new ModelId { ModelIndex = modelId });
            }
            
            //Ecs.AddComponent(en, new Vertex());

            
        }

        protected override void Render(double deltaTime)
        {
            
        }

        protected override void Update(double deltaTime)
        {
            
        }
    }
}
