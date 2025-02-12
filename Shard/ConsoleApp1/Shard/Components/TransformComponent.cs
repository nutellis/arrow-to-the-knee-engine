using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    class TransformComponent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Rotation { get; set; } // Rotation in degrees
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public string SpritePath { get; set; }

        public TransformComponent(float x, float y, float rotation = 0, float scaleX = 1.0f, float scaleY = 1.0f, string spritePath = null)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            ScaleX = scaleX;
            ScaleY = scaleY;
            SpritePath = spritePath;
        }

        // Translate the transform by dx and dy
        public void Translate(float dx, float dy)
        {
            X += dx;
            Y += dy;
        }

        // Rotate the transform by a given amount
        public void Rotate(float delta)
        {
            Rotation += delta;
            Rotation %= 360; // Ensure rotation is within 0-360 degrees
        }

        // Scale the transform by a given factor for both X and Y
        public void Scale(float scaleX, float scaleY)
        {
            ScaleX *= scaleX;
            ScaleY *= scaleY;
        }

        // Optionally, use this for more advanced transformations (if necessary)
        public Matrix4x4 GetTransformationMatrix()
        {
            float radians = MathF.PI * Rotation / 180f;
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(X, Y, 0);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationZ(radians);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(ScaleX, ScaleY, 1);

            return scaleMatrix * rotationMatrix * translationMatrix; // Order of operations is important!
        }
    }
}
