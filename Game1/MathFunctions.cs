using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace Game1
{
    public class MathFunctions
    {
        //Finds the intersection of vecA + vecS and vecB + vecT
        static public (Vector2, float) FindIntersection (Vector2 vecA, Vector2 vecS, Vector2 vecB, Vector2 vecT)
        {
            float a1 = vecA.X; float a2 = vecA.Y;
            float s1 = vecS.X; float s2 = vecS.Y;
            float t1 = vecT.X; float t2 = vecT.Y;  
            float b1 = vecB.X; float b2 = vecB.Y;

            float q = (((a1-b1)*s2) - ((a2-b2)*s1))/(t1*s2 - t2*s1);
            Vector2 Intersec = vecB + q * vecT;
            
            return (Intersec,q);
        }

        //finds the s int the system of equasions vecA + s* vecB = vecC + t*vecD
        static public float IntersecFindS(Vector2 vecA, Vector2 vecB, Vector2 vecC, Vector2 vecD)
        {
            float a1 = vecA.X; float a2 = vecA.Y;
            float b1 = vecB.X; float b2 = vecB.Y;
            float c1 = vecC.X; float c2 = vecC.Y;
            float d1 = vecD.X; float d2 = vecD.Y;

            float s = (d2 * (a1 - c1) + d2 * (c2 - a2)) / (b2 * d1 - b1 * d2);
            return s;

        }

        //rotates a vector by the given degrees (radiant)
        static public Vector2 RotateBy (Vector2 vec, float angle)
        {
            float Xq = vec.X;
            float Yq = vec.Y;
            float newVecX = (float)((float)(Xq * Cos(angle)) - ((float)Yq * Sin(angle)));
            float newVecY = (float)((float)(Xq * Sin(angle)) + (float)Yq * Cos(angle));
            Vector2 newVec = new Vector2(newVecX, newVecY);
            return newVec;

        }
    }
}
