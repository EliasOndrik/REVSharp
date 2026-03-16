using REVSharp.Components;
using REVSharp.Core;
using Silk.NET.Maths;

namespace SuperCubeBros
{
    internal class Colision : Behaviour
    {
        

        public override void OnRender(double deltaTime, ECS componentManager)
        {
            
        }

        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            foreach (var entity in Entities)
            {
                ref ColisionBox colisionBox = ref componentManager.GetComponent<ColisionBox>(in entity);
                if (colisionBox.Type == ColisionType.Static)
                {
                    continue;
                }
                ref Transform transform = ref componentManager.GetComponent<Transform>(in entity);
                colisionBox.Direction = ColisionDirection.None;
                foreach (var otherEntity in Entities)
                {
                    if (entity.Id == otherEntity.Id)
                    {
                        continue;
                    }
                    ref Transform otherTransform = ref componentManager.GetComponent<Transform>(in otherEntity);
                    ref ColisionBox otherColisionBox = ref componentManager.GetComponent<ColisionBox>(in otherEntity);
                    
                    Vector3D<float> differece = otherTransform.Position - transform.Position;

                    SimpleColisionBox apliedBox = new()
                    {
                        Left = transform.Position.X + colisionBox.Box.Left * transform.Scale.X,
                        Right = transform.Position.X + colisionBox.Box.Right * transform.Scale.X,
                        Bottom = transform.Position.Y + colisionBox.Box.Bottom * transform.Scale.Y,
                        Top = transform.Position.Y + colisionBox.Box.Top * transform.Scale.Y,
                        Back = transform.Position.Z + colisionBox.Box.Back * transform.Scale.Z,
                        Front = transform.Position.Z + colisionBox.Box.Front * transform.Scale.Z
                    };
                    SimpleColisionBox otherApliedBox = new()
                    {
                        Left = otherTransform.Position.X + otherColisionBox.Box.Left * otherTransform.Scale.X,
                        Right = otherTransform.Position.X + otherColisionBox.Box.Right * otherTransform.Scale.X,
                        Bottom = otherTransform.Position.Y + otherColisionBox.Box.Bottom * otherTransform.Scale.Y,
                        Top = otherTransform.Position.Y + otherColisionBox.Box.Top * otherTransform.Scale.Y,
                        Back = otherTransform.Position.Z + otherColisionBox.Box.Back * otherTransform.Scale.Z,
                        Front = otherTransform.Position.Z + otherColisionBox.Box.Front * otherTransform.Scale.Z
                    };

                    if (apliedBox.Left <= otherApliedBox.Right &&
                        apliedBox.Right >= otherApliedBox.Left &&
                        apliedBox.Bottom <= otherApliedBox.Top &&
                        apliedBox.Top >= otherApliedBox.Bottom &&
                        apliedBox.Back <= otherApliedBox.Front &&
                        apliedBox.Front >= otherApliedBox.Back)
                    {
                        float overlapX = Math.Min(apliedBox.Right - otherApliedBox.Left, otherApliedBox.Right - apliedBox.Left);
                        float overlapY = Math.Min(apliedBox.Top - otherApliedBox.Bottom, otherApliedBox.Top - apliedBox.Bottom);
                        float overlapZ = Math.Min(apliedBox.Front - otherApliedBox.Back, otherApliedBox.Front - apliedBox.Back);
                        Vector3D<float> corection = Vector3D<float>.Zero;

                        if (overlapX <= overlapY && overlapX <= overlapZ)
                        {
                            if (differece.X > 0)
                            {
                                corection.X = -overlapX;
                                colisionBox.Direction |= ColisionDirection.Right;
                            }
                            else
                            {
                                corection.X = overlapX;
                                colisionBox.Direction |= ColisionDirection.Left;
                            }
                        }
                        if (overlapY <= overlapX && overlapY <= overlapZ)
                        {
                            if (differece.Y > 0)
                            {
                                corection.Y = -overlapY;
                                colisionBox.Direction |= ColisionDirection.Top;
                            }
                            else
                            {
                                corection.Y = overlapY;
                                colisionBox.Direction |= ColisionDirection.Bottom;
                            }
                        }
                        if (overlapZ <= overlapY && overlapZ <= overlapX)
                        {
                            if (differece.Z > 0)
                            {
                                corection.Z = -overlapZ;
                                colisionBox.Direction |= ColisionDirection.Front;
                            }
                            else
                            {
                                corection.Z = overlapZ;
                                colisionBox.Direction |= ColisionDirection.Back;
                            }
                        }

                        transform.Position += corection;
                    }
                }
            }
        }
    }
}
