/*
*
*   The specific collider for rectangles.   Handles rect/circle, rect/rect and rect/vector.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Drawing;
using System.Numerics;

namespace Shard
{
    class ColliderRect : Collider
    {
        private Transform myRect;
        private float baseWid, baseHt;
        private float x, y, wid, ht;
        private bool fromTrans;



        public ColliderRect(CollisionHandler gob, Transform t) : base(gob)
        {

            this.MyRect = t;
            fromTrans = true;
            RotateAtOffset = false;
            calculateBoundingBox();
        }

        public ColliderRect(CollisionHandler gob, Transform t, float x, float y, float wid, float ht) : base(gob)
        {
            X = x;
            Y = y;
            BaseWid = wid;
            BaseHt = ht;
            RotateAtOffset = true;

            this.MyRect = t;

            fromTrans = false;

        }

        public void calculateBoundingBox()
        {
            float nwid, nht, angle, x1, x2, y1, y2;
            double cos, sin;
            if (myRect == null)
            {
                return;
            }

            if (fromTrans)
            {
                Wid = (float)(MyRect.Wid * MyRect.Scalex);
                Ht = (float)(MyRect.Ht * MyRect.Scaley);
            }
            else
            {
                Wid = (float)(BaseWid * MyRect.Scalex);
                Ht = (float)(BaseHt * MyRect.Scaley);
            }

            angle = (float)(Math.PI * MyRect.Rotz / 180.0f);


            cos = Math.Cos(angle);
            sin = Math.Sin(angle);

            // Bit of trig here to calculate the new height and width
            nwid = (float)(Math.Abs(Wid * cos) + Math.Abs(Ht * sin));
            nht = (float)(Math.Abs(Wid * sin) + Math.Abs(Ht * cos));

            X = (float)MyRect.X + (Wid / 2);
            Y = (float)MyRect.Y + (Ht / 2);

            Wid = nwid;
            Ht = nht;

            if (RotateAtOffset) {
                // Now we work out the X and Y based on the rotation of the body to 
                // which this belongs,.
                x1 = X - MyRect.Centre.X;
                y1 = Y - MyRect.Centre.Y;

                x2 = (float)(x1 * Math.Cos(angle) - y1 * Math.Sin(angle));
                y2 = (float)(x1 * Math.Sin(angle) + y1 * Math.Cos(angle));

                X = x2 + (float)MyRect.Centre.X;
                Y = y2 + (float)MyRect.Centre.Y;
            }

            MinAndMaxX[0] = X - Wid / 2;
            MinAndMaxX[1] = X + Wid / 2;
            MinAndMaxY[0] = Y - Ht / 2;
            MinAndMaxY[1] = Y + Ht / 2;


        }

        internal Transform MyRect { get => myRect; set => myRect = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Wid { get => wid; set => wid = value; }
        public float Ht { get => ht; set => ht = value; }
        public float Left { get => MinAndMaxX[0]; set => MinAndMaxX[0] = value; }
        public float Right { get => MinAndMaxX[1]; set => MinAndMaxX[1] = value; }
        public float Top { get => MinAndMaxY[0]; set => MinAndMaxY[0] = value; }
        public float Bottom { get => MinAndMaxY[1]; set => MinAndMaxY[1] = value; }
        public float BaseWid { get => baseWid; set => baseWid = value; }
        public float BaseHt { get => baseHt; set => baseHt = value; }

        public override void recalculate()
        {
            calculateBoundingBox();
        }

        public ColliderRect calculateMinkowskiDifference(ColliderRect other)
        {
            float left, right, top, bottom, width, height;
            ColliderRect mink = new ColliderRect(null, null);

            // A set of calculations that gives us the Minkowski difference
            // for this intersection.
            left = Left - other.Right;
            top = other.Top - Bottom;
            width = Wid + other.Wid;
            height = Ht + other.Ht;
            right = Right - other.Left;
            bottom = other.Bottom - Top;

            mink.Wid = width;
            mink.Ht = height;

            mink.MinAndMaxX = new float[2] { left, right };
            mink.MinAndMaxY = new float[2] { top, bottom };

            return mink;
        }

        public Vector2? calculatePenetration(Vector2 checkPoint)
        {
            Vector2? impulse;
            float coff = 0.2f;

            // Check the right edge
            float min;

            min = Math.Abs(Right - checkPoint.X);
            impulse = new Vector2(-1 * min - coff, checkPoint.Y);


            // Now compare against the Left edge
            if (Math.Abs(checkPoint.X - Left) <= min)
            {
                min = Math.Abs(checkPoint.X - Left);
                impulse = new Vector2(min + coff, checkPoint.Y);
            }

            // Now the bottom
            if (Math.Abs(Bottom - checkPoint.Y) <= min)
            {
                min = Math.Abs(Bottom - checkPoint.Y);
                impulse = new Vector2(checkPoint.X, min + coff);
            }

            // And now the top
            if (Math.Abs(Top - checkPoint.Y) <= min)
            {
                min = Math.Abs(Top - checkPoint.Y);
                impulse = new Vector2(checkPoint.X, -1 * min - coff);
            }

            return impulse;
        }

        public override Vector2? checkCollision(ColliderRect other)
        {
            ColliderRect cr;

            cr = calculateMinkowskiDifference(other);

            if (cr.Left <= 0 && cr.Right >= 0 && cr.Top <= 0 && cr.Bottom >= 0)
            {
                return cr.calculatePenetration(new Vector2(0, 0));
            }



            return null;

        }

        public override void drawMe(Color col)
        {
            Display d = Bootstrap.getDisplay();

            d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[0], (int)MinAndMaxX[1], (int)MinAndMaxY[0], col);
            d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[0], (int)MinAndMaxX[0], (int)MinAndMaxY[1], col);
            d.drawLine((int)MinAndMaxX[1], (int)MinAndMaxY[0], (int)MinAndMaxX[1], (int)MinAndMaxY[1], col);
            d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[1], (int)MinAndMaxX[1], (int)MinAndMaxY[1], col);

            d.drawCircle((int)X, (int)Y, 2, col);
        }

        public override Vector2? checkCollision(ColliderCircle c)
        {
            Vector2? possibleV = c.checkCollision(this);

            if (possibleV is Vector2 v)
            {
                v.X *= -1;
                v.Y *= -1;
                return v;
            }

            return null;
        }

        public override float[] getMinAndMaxX()
        {
            return MinAndMaxX;
        }

        public override float[] getMinAndMaxY()
        {
            return MinAndMaxY;
        }

        public override Vector2? checkCollision(Vector2 other)
        {

            if (other.X >= Left &&
                other.X <= Right &&
                other.Y >= Top &&
                other.Y <= Bottom)
            {
                return new Vector2(0, 0);
            }

            return null;
        }

    }


}
