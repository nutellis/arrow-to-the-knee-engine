using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class TransformComponent : Component
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Rotation { get; set; } // Rotation in degrees
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public string SpritePath { get; set; }

        public TransformComponent(GameObject owner, float x, float y, float rotation = 0, float scaleX = 1.0f, float scaleY = 1.0f) : base(owner)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            ScaleX = scaleX;
            ScaleY = scaleY;
            SpritePath = string.Empty;
            Width = 0;
            Height = 0;
        }

        public Transform3D transform;

        // set transform or whatever

        public void initialize(float x, float y)
        {
            transform.X = x;
            transform.Y = y;
        }

        public override void update()
        {

        }

        internal Transform3D Transform
        {
            get => transform;
        }

        internal Transform Transform2D
        {
            get => (Transform)transform;
        }

        public float getCenterX()
        {
            return transform.Centre.X;
        }
        

        // Translate the transform by dx and dy
        public void translate(float dx, float dy)
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

        //protected override void UpdateComponent()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
