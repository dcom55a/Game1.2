using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class PhyModel
    {
        public List<PhyObject> ObjList = new List<PhyObject>();

        //Adds an Object to the List
        public void AddObj(PhyObject Obj)
        {
            ObjList.Add(Obj);
        }

        //Creates a new Object and Adss it to the List
        public void CreateAndAdd(string name,  Hitbox hitbox, Texture2D texture, Vector2 velocity, Vector2 position, int collisionLayer, bool moveable) 
        {
            PhyObject ObjToAdd = new PhyObject(name, hitbox, texture, velocity, position, collisionLayer, moveable);
            ObjList.Add(ObjToAdd);
        }

        //removes an Object from the List
        public void RemoveObj(PhyObject Object)
        {
            ObjList.Remove(Object);
        }

        //removes all Objects with the specified name from the List
        public void RemoveObj(string name)
        {
            List<PhyObject> ObjNotToRem = new List<PhyObject>();
            foreach(PhyObject Obj in ObjList)
            {
                if(Obj.Name != name) { ObjNotToRem.Add(Obj); }
            }
            ObjList.Clear();
            ObjList = ObjNotToRem;
        }
        
    }
}
