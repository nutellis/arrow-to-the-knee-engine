﻿/*
*
*   The transform class handles position, independent of physics and forces (although the physics
*       system will make use of the rotation and translation functions here).  Essentially this class
*       is a game object's location (X, Y), rotation and scale.  Usefully it also calculates the 
*       centre of an object as well as relative directions such as forwards and right.  If you want 
*       backwards and left, multiply forward or right by -1.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/


using System;
using System.Numerics;

namespace Shard
{

    public class Transform
    {
        private float x, y;
        private float lx, ly;
        private float rotz;
        private int wid, ht;
        private float scalex, scaley;

        private Vector2 forward;
        private Vector2 right, centre;

        public Vector2 getLastDirection()
        {
            float dx, dy;
            dx = (X - Lx);
            dy = (Y - Ly);

            return new Vector2(-dx, -dy);
        }

        public Transform()
        {
            forward = new Vector2();
            right = new Vector2();
            centre = new Vector2();

            scalex = 1.0f;
            scaley = 1.0f;

            x = 0;
            y = 0;

            lx = 0;
            ly = 0;

            rotate(0);
        }


        public void recalculateCentre()
        {

            centre.X = (float)(x + ((this.Wid * scalex) / 2));
            centre.Y = (float)(y + ((this.Ht * scaley) / 2));

        }

        public void translate(double nx, double ny)
        {
            translate ((float)nx, (float)ny);
        }



        public void translate(float nx, float ny)
        {

            lx += (float)nx;
            ly += (float)ny;

            //if (lx != 0 && ly != 0)
            //{
            //    // normalize the movement vector to ensure consistent speed
            //    float magnitude = (float)Math.Sqrt(Lx * Lx + Ly * Ly);
            //    if (magnitude > 0)
            //    {
            //        lx = lx * (Math.Abs(lx) / magnitude);
            //        ly = ly * (Math.Abs(ly) / magnitude);
            //    }
            //}
        }

        public void translate(Vector2 vect)
        {
            translate(vect.X, vect.Y);
        }

        public void consumeMovement()
        {
            X += Lx;
            Y += Ly;

            Lx = 0;
            Ly = 0;

            recalculateCentre();
        }

        public void moveImmediately(float nx, float ny)
        {
            translate(nx, ny);
            consumeMovement();
        }

        public float magnitude()
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public Transform normalized()
        {
            float mag = magnitude();
            var newVect = new Transform();
            if (mag > 0)
            {
                newVect.x = x / mag;
                newVect.y = y / mag;
            }
            return newVect;
        }

        public static float distance(Transform a, Transform b)
        {
            float dx = b.x - a.x;
            float dy = b.y - a.y;
            return (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }

        public static float distance(float a, float b)
        {
            float delta = b - a;
            return (float)Math.Abs(delta);
        }

        public void rotate(float dir)
        {
            rotz += (float)dir;

            rotz %= 360;

            float angle = (float)(Math.PI * rotz / 180.0f);
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            forward.X = cos;
            forward.Y = sin;


            right.X = -1 * forward.Y;
            right.Y = forward.X;




        }

        public static Transform lerp(Transform a, Transform b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);

            Transform result = new Transform();

            result.X = a.X + (b.X - a.X) * t;
            result.Y = a.Y + (b.Y - a.Y) * t;

            result.Rotz = a.Rotz + (b.Rotz - a.Rotz) * t;
            result.Scalex = a.Scalex + (b.Scalex - a.Scalex) * t;
            result.Scaley = a.Scaley + (b.Scaley - a.Scaley) * t;

            return result;
        }

        public float X
        {
            get => x;
            set => x = value;
        }
        public float Y
        {
            get => y;
            set => y = value;
        }

        public float Rotz
        {
            get => rotz;
            set => rotz = value;
        }

        public ref Vector2 Forward { get => ref forward; }
        public int Wid { get => wid; set => wid = value; }
        public int Ht { get => ht; set => ht = value; }
        public ref Vector2 Right { get => ref right; }
        public ref Vector2 Centre { get => ref centre; }
        public float Scalex { get => scalex; set => scalex = value; }
        public float Scaley { get => scaley; set => scaley = value; }
        public float Lx { get => lx; set => lx = value; }
        public float Ly { get => ly; set => ly = value; }
    }
}
