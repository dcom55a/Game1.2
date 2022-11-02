using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    internal class InputEventHandler
    {
        Vector2 startPos;
        bool justPressed = false;
        Vector2 dirVec;
        public bool wDown = false;
        public bool WPressed()
        {
            var keyState = Keyboard.GetState();
            
            if (keyState.GetPressedKeys().Contains(Keys.W))
            {
                return true; 
            }
            return false;
        }
        public Vector2 GetSwing()
        {
            var mouseState = Mouse.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if (justPressed == false)
                {
                    startPos = mouseState.Position.ToVector2();
                    justPressed = true;
                }
                else
                {
                    dirVec = mouseState.Position.ToVector2() - startPos;  
                }
                
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (justPressed == true)
                {
                    
                    justPressed = false;
                    return dirVec;
                }
            }

            return Vector2.Zero;
        }
    }
}
