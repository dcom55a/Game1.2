using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class PhyObject
    {
        public string Name { get; set; }
        public Hitbox Hitbox { get; set; }
        public Texture2D Texture { get; set; }
        //sets a collisionLayer between 0 and 2
        public int CollisionLayer { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public bool Moveable;

        public PhyObject(string name, Hitbox hitbox,Texture2D texture, Vector2 velocity, Vector2 position, int collisionLayer, bool affGravity)
        {
            Name = name;
            Hitbox = hitbox;
            Texture = texture;
            Velocity = velocity;
            Position = position;
            CollisionLayer = collisionLayer;
            Moveable = affGravity;

        }


    }
}
