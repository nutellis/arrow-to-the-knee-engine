/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System;

namespace Shard
{
    public class SoundSDL : Sound
    {


        public override void playSound(string file)
        {
            SDL.SDL_AudioSpec have, want;
            uint length, dev;
            IntPtr buffer;

            
            file = Bootstrap.getAssetManager().getAssetPath(file);


            SDL.SDL_LoadWAV(file, out have, out buffer, out length);

            dev = SDL.SDL_OpenAudioDevice(IntPtr.Zero, 0, ref have, out want, 0);

            if (SDL.SDL_GetQueuedAudioSize(dev) >= 15)
            {
                SDL.SDL_CloseAudioDevice(dev);
                dev = SDL.SDL_OpenAudioDevice(IntPtr.Zero, 0, ref have, out want, 0);
            }

            int success = SDL.SDL_QueueAudio(dev, buffer, length);
            SDL.SDL_PauseAudioDevice(dev, 0);

            SDL.SDL_FreeWAV(buffer);

        }

    }
}

