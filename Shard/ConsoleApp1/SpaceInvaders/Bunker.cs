using Shard;

namespace SpaceInvaders
{
    class Bunker : GameObject
    {
        private int[,] bits;

        public Bunker()
        {
            bits = new int[4, 5] {
                { 0, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 1 }
            };


        }

        public void setupBunker()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (bits[i, j] == 0)
                    {
                        continue;
                    }

                    BunkerBit myBit = new BunkerBit();
                    myBit.transform.X = this.transform.X + (j * 16);
                    myBit.transform.Y = this.transform.Y + (i * 16);
                }

            }

        }
    }
}
