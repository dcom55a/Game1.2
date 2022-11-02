using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class PhysicsHandler : MathFunctions
    {
        public PhyModel phyModel;
        public int windowHeight { get; set; }
        public int windowWidth { get; set; }

        public PhysicsHandler(int windowWidth, int windowHeight)
        {
            phyModel = new PhyModel();
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
        }
        public (Vector2, Vector2) CalculateBallMovement(Vector2 velocity, Vector2 sprBallPos, GameTime gameTime, Vector2 swing)
        {
            //if Ball is stil and the player swang, set the velocity to the swing
            if (swing != Vector2.Zero && velocity.Length() < 1)
            {
                velocity = (-2) * swing;
            }

            //Gravity
            if (sprBallPos.Y <= windowHeight - 18)
            {
                velocity = velocity + new Vector2(0, 500f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                //Stops ball so that it doesnt bounce forever, if bpounces get to small
                if (velocity.Y < 75f && velocity.Y > (-1) * 75f)
                {
                    velocity.Y = 0;
                    sprBallPos.Y = windowHeight - 15;
                }
            }
            //stops ball if movement in x dir gets to small
            if (velocity.X < 5f && velocity.X > -5f)
            {
                velocity.X = 0;
            }

            //checks if ball is out of bounds
            if (sprBallPos.X < 1 || sprBallPos.X > windowWidth - 15)
            { velocity.X = (-0.95f) * velocity.X; }
            if (sprBallPos.Y > windowHeight - 15 || sprBallPos.Y < 1)
            {
                velocity.Y = (-0.95f) * velocity.Y;
                if (sprBallPos.Y > windowHeight - 15)
                {
                    sprBallPos.Y = windowHeight - 15;
                }
            }
            velocity = velocity * 0.99f;
            sprBallPos += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            return (sprBallPos, velocity);
        }

        public List<PhyObject> UpdatePhysics(GameTime gameTime, Vector2 swing)
        {
            List<PhyObject> newList = new List<PhyObject>();
            foreach (PhyObject obj in phyModel.ObjList)
            {

                if (obj.Name.ToLower() == "player")
                {
                    //changes velocity according to playernput id there is one
                    if (swing != Vector2.Zero && obj.Velocity.Length() < 1)
                    {
                        obj.Velocity = (-2) * swing;
                    }
                }

                //gravity and friction
                if (obj.Moveable == true)
                {
                    if (obj.Position.Y <= windowHeight - 18)
                    {
                        obj.Velocity = obj.Velocity + new Vector2(0, 500f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    //Stops object so that it doesnt bounce forever, if bpounces get to small
                    else
                    {
                        if (obj.Velocity.Y < 75f && obj.Velocity.Y > (-1) * 75f && obj.Velocity.Y != 0f)
                        {
                            float Xvel = (float)obj.Velocity.X;
                            obj.Velocity = new Vector2(Xvel, 0);
                        }
                    }

                    //stops obj if movement in x dir gets to small
                    if (obj.Velocity.X < 5f && obj.Velocity.X > -5f)
                    {
                        float Yvel = (float)obj.Velocity.Y;
                        obj.Velocity = new Vector2(0, Yvel);
                    }

                    //checks if object is out of bounds and reverses the velocity in the direction of border collision
                    if (obj.Position.X < 1 || obj.Position.X > windowWidth - 15)
                    {
                        obj.Velocity = new Vector2((-0.95f) * obj.Velocity.X, obj.Velocity.Y);
                        if (obj.Position.X < 1)
                        {
                            obj.Position = new Vector2(1,obj.Position.Y);
                        }
                        else
                        {
                            obj.Position = new Vector2(windowWidth-15,obj.Position.Y);
                        }
                    }
                    if (obj.Position.Y > windowHeight - 15 || obj.Position.Y < 1)
                    {
                        obj.Velocity = new Vector2(obj.Velocity.X, (-0.95f) * obj.Velocity.Y);
                        if (obj.Position.Y > windowHeight - 15)
                        {
                            obj.Position = new Vector2(obj.Position.X, windowHeight - 15);
                        }
                        else
                        {
                            obj.Position = new Vector2(obj.Position.X, 1);
                        }
                    }

                    //checks potentail collisions with non movable objects and handels the collision
                    foreach (PhyObject PotCol in phyModel.ObjList)
                    {
                        if (PotCol.Moveable == false)
                        {
                            Vector2 newPosObj = obj.Position + obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                            switch (PotCol.Hitbox.Shape)
                            {
                                //circle on cirlce
                                case "circle":
                                    if ((newPosObj - PotCol.Position).Length() < PotCol.Hitbox.Height + obj.Hitbox.Height)
                                    {
                                        //collision Handeling
                                        obj.Velocity = obj.Velocity * (-1);
                                    }


                                    break;
                                case "square":

                                    break;
                            }
                        }
                    }

                    obj.Velocity = obj.Velocity * 0.99f;
                    obj.Position += obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
            }
            return phyModel.ObjList;   
        }

        public List<(PhyObject,PhyObject)> GetCollisions()
        {
            List<(PhyObject,PhyObject)> returnList = new List<(PhyObject,PhyObject)> ();

            foreach (PhyObject Obj in phyModel.ObjList)
            {
                if (Obj.Moveable)
                {
                    foreach(PhyObject PotCol in phyModel.ObjList)
                    {
                        if (!Obj.Moveable)
                        {
                            float aM1 = Obj.Position.X;
                            float aM2 = Obj.Position.Y;
                            float bM1 = PotCol.Position.X;
                            float bM2 = PotCol.Position.Y;
                            float bHeight = PotCol.Hitbox.Height;
                            float? bWidth = PotCol.Hitbox.Width;
                            float rA = Obj.Hitbox.Height;
                            Vector2 v = Obj.Velocity;
                            float vL = Obj.Velocity.Length();

                            if (bM1 - bWidth / 2 <= aM1 + (1 + (rA / vL)) * v.X
                                && aM1 + (1 + (rA / vL)) * v.X <= bM1 + bWidth
                                && (bM2 - (bHeight / 2)) <= aM2 + (1 + (rA / vL)) * v.Y
                                && aM2 + (1 + (rA / vL)) * v.Y <= bM2 + bHeight)
                            {
                                (PhyObject, PhyObject) CollisionPair = (Obj, PotCol);
                                returnList.Add(CollisionPair);
                            }
                        }
                    }
                }
            }
            return returnList;
        }
        public void HandleCollisions(List<(PhyObject,PhyObject)> CollisionList)
        {
            foreach ((PhyObject,PhyObject) Collision in CollisionList)
            {
                PhyObject Obj = Collision.Item1;
                PhyObject CollObj = Collision.Item2;

                Vector2 a = Obj.Position;
                Vector2 c = CollObj.Position;
                Vector2 b = Obj.Velocity;
                Vector2 bC1 = CollObj.Position + new Vector2((float)-CollObj.Hitbox.Width, -CollObj.Hitbox.Height);
                Vector2 bC2 = CollObj.Position + new Vector2((float)CollObj.Hitbox.Width, -CollObj.Hitbox.Height);
                Vector2 bC3 = CollObj.Position + new Vector2((float)CollObj.Hitbox.Width, CollObj.Hitbox.Height);
                Vector2 bC4 = CollObj.Position + new Vector2((float)-CollObj.Hitbox.Width, CollObj.Hitbox.Height);

                //float bHeight = CollObj.Hitbox.Height;
                //float? bWidth = CollObj.Hitbox.Width;
                //float rA = Obj.Hitbox.Height;

                //float vL = Obj.Velocity.Length();

                //checks Collision with Lower and Left Edge of Hitbox if Ball is moving to pos X Direction and neg Y Direction
                if (b.X > 0 && b.Y < 0)
                {
                    Vector2 C34 = bC3 - bC4;
                    Vector2 C41 = bC4 - bC1;

                    //solving the equation system a(vec)+s*b(vec) = c(vec)+ t* d(vec) for s 
                    float s_C34 = IntersecFindS(a, b, c, C34);
                    float s_C41 = IntersecFindS(a, b, c, C41);

                    //inverts Y velocity if ball collides with Lower Edege of Hitbox
                    if (s_C34 < 1 && s_C34 > 0)
                    {
                        Obj.Velocity =  new Vector2(Obj.Velocity.X, (-2) * Obj.Velocity.Y) * 0.95f;
                    }

                    //inverts X velocity if ball collides with Left Edge of Hitbox
                    if (s_C41 < 1 && s_C41 > 0)
                    {
                        Obj.Velocity = new Vector2((-2) * Obj.Velocity.X, Obj.Velocity.Y) * 0.95f;
                    }
                }

                //checks Collision with Upper (C12) and Left(C41) Edge of Hitbox if Ball is moving to pos X Direction and neg Y Direction
                if (b.X > 0 && b.Y > 0)
                {
                    Vector2 C12 = bC1 - bC2;
                    Vector2 C41 = bC4 - bC1;

                    //solving the equation system a(vec)+s*b(vec) = c(vec)+ t* d(vec) for s
                    float s_C41 = IntersecFindS(a, b, c, C41);
                    float s_C12 = IntersecFindS(a, b, c, C12);

                    //inverts Y velocity if ball collides with Upper Edege of Hitbox and simulates friction by decreasing Velocity
                    if (s_C12 < 1 && s_C12 > 0)
                    {
                        Obj.Velocity = new Vector2(Obj.Velocity.X, (-2) * Obj.Velocity.Y)*0.95f;
                    }

                    //inverts X velocity if ball collides with Left Edge of Hitbox and simulates friction by decreasing Velocity
                    if (s_C41 < 1 && s_C41 > 0)
                    {
                        Obj.Velocity = new Vector2((-2) * Obj.Velocity.X, Obj.Velocity.Y) * 0.95f;
                    }
                }

                //checks Collision with Upper (C12) and Right (C23) Edge of Hitbox if Ball is moving to pos X Direction and neg Y Direction
                if (b.X < 0 && b.Y > 0)
                {
                    Vector2 C12 = bC1 - bC2;
                    Vector2 C23 = bC2 - bC3;

                    //solving the equation system a(vec)+s*b(vec) = c(vec)+ t* d(vec) for s
                    float s_C23 = IntersecFindS(a, b, c, C23);
                    float s_C12 = IntersecFindS(a, b, c, C12);

                    //inverts Y velocity if ball collides with Upper Edege of Hitbox and simulates friction by decreasing Velocity
                    if (s_C12 < 1 && s_C12 > 0)
                    {
                        Obj.Velocity = new Vector2(Obj.Velocity.X, (-2) * Obj.Velocity.Y) * 0.95f;
                    }

                    //inverts X velocity if ball collides with Right Edge of Hitbox and simulates friction by decreasing Velocity
                    if (s_C23 < 1 && s_C23 > 0)
                    {
                        Obj.Velocity = new Vector2((-2) * Obj.Velocity.X, Obj.Velocity.Y) * 0.95f;
                    }
                }

                //checks Collision with Upper (C12) and Right (C23) Edge of Hitbox if Ball is moving to pos X Direction and neg Y Direction
                if (b.X < 0 && b.Y < 0)
                {
                    Vector2 C34 = bC3 - bC4;
                    Vector2 C23 = bC2 - bC3;

                    //solving the equation system a(vec)+s*b(vec) = c(vec)+ t* d(vec) for s
                    float s_C23 = IntersecFindS(a, b, c, C23);
                    float s_C34 = IntersecFindS(a, b, c, C34);

                    //inverts Y velocity if ball collides with Upper Edege of Hitbox and simulates friction by decreasing Velocity
                    if (s_C34 < 1 && s_C34 > 0)
                    {
                        Obj.Velocity = new Vector2(Obj.Velocity.X, (-2) * Obj.Velocity.Y) * 0.95f;
                    }

                    //inverts X velocity if ball collides with Right Edge of Hitbox and simulates friction by decreasing Velocity
                    if (s_C23 < 1 && s_C23 > 0)
                    {
                        Obj.Velocity = new Vector2((-2) * Obj.Velocity.X, Obj.Velocity.Y) * 0.95f;
                    }
                }


            }
        }
    }
}
