/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    public class Sound
    {
        public string soundName;
        public double length;
        public bool isPlaying;
        public IntPtr soundwave;

        public Sound(string soundName, double length, bool isPlaying, nint soundwave)
        {
            this.soundName = soundName;
            this.length = length;
            this.isPlaying = isPlaying;
            this.soundwave = soundwave;
        }
    }
}
