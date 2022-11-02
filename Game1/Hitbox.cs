using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Hitbox
    {
        
        public float Height { get; set; }
        public float? Width { get; set; }
        public string Shape { get; set; }

        public Hitbox(string shape, float height, float? width = null)
        {
            
            this.Height = height;
            this.Width = width;
            this.Shape = shape;
            if (shape == "circle")
            {
                this.Width = null;
            }
        }
    }
}
