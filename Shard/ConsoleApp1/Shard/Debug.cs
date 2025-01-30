/*
*
*   Some handy debug methods.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    class Debug
    {
        public static int DEBUG_LEVEL_NONE = 0;
        public static int DEBUG_LEVEL_ERROR = 1;
        public static int DEBUG_LEVEL_WARNING = 2;
        public static int DEBUG_LEVEL_ALL = 3;

        private static Debug me;
        private int debugLevel;

        private Debug()
        {
            debugLevel = DEBUG_LEVEL_ALL;
        }

        public static Debug getInstance()
        {
            if (me == null)
            {
                me = new Debug();
            }

            return me;
        }

        public void setDebugLevel(int d)
        {
            debugLevel = d;
        }

        public void log(string message, int level)
        {
            if (debugLevel == DEBUG_LEVEL_NONE)
            {
                return;
            }

            if (level <= debugLevel)
            {
                Console.WriteLine(message);
            }
        }

        public void log(string message)
        {
            log(message, DEBUG_LEVEL_ALL);
        }


        public static void Log(string message)
        {
            getInstance().log(message, DEBUG_LEVEL_ALL);
        }

    }
}
