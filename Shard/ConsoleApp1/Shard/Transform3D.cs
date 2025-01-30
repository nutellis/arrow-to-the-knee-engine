/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    class Transform3D : Transform
    {
        private double z;
        private double rotx, roty;
        private int scalez;

        public Transform3D(GameObject o) : base(o)
        {
        }

        public double Z
        {
            get => z;
            set => z = value;
        }



        public int Scalez
        {
            get => scalez;
            set => scalez = value;
        }
        public double Rotx { get => rotx; set => rotx = value; }
        public double Roty { get => roty; set => roty = value; }
    }
}
