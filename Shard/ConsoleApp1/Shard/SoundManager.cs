using SDL2;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_mixer;

namespace Shard.Shard
{
    internal class SoundManager
    {

        static ushort mixerFormat = SDL_mixer.MIX_DEFAULT_FORMAT;
        static Int32 frequency = SDL_mixer.MIX_DEFAULT_FREQUENCY;
        static Int32 channels = SDL_mixer.MIX_DEFAULT_CHANNELS;
        static Int32 chunckSize = 2048;

        private static SoundManager me;
        private Dictionary<string, Sound> soundLibrary;

        private Dictionary<string, int> occupiedChannels; // Store sound -> channel mapping

        private SoundManager()
        {
            if (SDL_mixer.Mix_OpenAudio(frequency, mixerFormat, channels, chunckSize) < 0)
            {
                Console.WriteLine($"SDL_mixer could not initialize! SDL_mixer Error: {SDL.SDL_GetError()}");
            }

            soundLibrary = new Dictionary<string, Sound>();
            occupiedChannels = new Dictionary<string, int>(); // Initialize tracking
        }

        public static SoundManager getInstance()
        {
            if (me == null)
            {
                me = new SoundManager();
            }

            return me;
        }

        public void loadSound(string soundName, string filePath)
        {
            if (soundLibrary.TryGetValue(soundName, out Sound value))
            {
                return;
            }

            filePath = Bootstrap.getAssetManager().getAssetPath(filePath);
  
            if (filePath == null)  // Check if file exists
            {
                Console.WriteLine($"ERROR: File not found -> {filePath}");
                return;
            }
            
            Console.WriteLine($"Loading sound: {soundName} from {filePath}");

            IntPtr soundwave = IntPtr.Zero;
            double seconds = 0.0;

            soundwave = SDL_mixer.Mix_LoadWAV(filePath);
            

            //soundwave = SDL.SDL_LoadWAV(filePath, out SDL.SDL_AudioSpec spec, out IntPtr soundBuff, out uint audioLen);

            
            if (soundwave != IntPtr.Zero)
            {
                SDL_mixer.MIX_Chunk chunk = Marshal.PtrToStructure<SDL_mixer.MIX_Chunk>(soundwave);

                uint audioLen = (uint)chunk.alen / 4;

                int sampleSize = SDL.SDL_AUDIO_BITSIZE(mixerFormat) / 8;

                uint sampleCount = audioLen / (uint)sampleSize;

                uint sampleLen = (channels > 0) ? (sampleCount / (uint)channels) : sampleCount;
                
                seconds = (double)sampleLen / frequency;
                
                Sound newSound = new Sound(soundName, seconds, false, soundwave);

                soundLibrary[soundName] = newSound;
            } else
            {
                Console.WriteLine($"Failed to load sound: {filePath}, SDL_mixer Error: {SDL.SDL_GetError()}");
                return;
            }
        }

        public async Task playSoundWithDelay(string soundName, int delay, bool loop = false, int time = 0)
        {
            await Task.Delay(delay); // Ensure the delay is awaited properly
            playSound(soundName, loop, time);
        }

        public int playSound(string soundName, bool loop = false, int time = 0)
        {
            //sound is already playing. Skip.
            if (occupiedChannels.ContainsKey(soundName))
            {
                return -1;
            }
            else
            {
                if (soundLibrary.TryGetValue(soundName, out Sound sound))
                {
                    if (sound.soundwave == IntPtr.Zero)
                    {
                        return -1;
                    }
                    int loops = loop ? -1 : 0;
                    int channel = SDL_mixer.Mix_PlayChannel(-1, sound.soundwave, loops);
                    if (channel != -1)
                    {
                        occupiedChannels[soundName] = channel; // Store channel for this sound

                        if (loop == false)
                        {
                            TimeSpan span = TimeSpan.FromSeconds(sound.length);
                            Task.Delay((int)span.TotalMilliseconds).ContinueWith(_ => stopSound(soundName));
                        }

                        if (time > 0)
                        {
                            Task.Delay(time).ContinueWith(_ => stopSound(soundName));
                        }
                    }
                    return channel;
                }
            }
            return -1;
        }

        public void stopAllSounds()
        {
            SDL_mixer.Mix_HaltChannel(-1);
        }

        public void setVolume(string soundName, float volume)
        {
            if(soundLibrary.TryGetValue(soundName, out Sound value)) { 

                    int sdlVolume = (int)(volume * 128);
                    SDL_mixer.Mix_VolumeChunk(value.soundwave, sdlVolume);
                    return;
            }
        }

        public void dispose()
        {
            foreach (var (_, sound) in soundLibrary)
            {
                SDL_mixer.Mix_FreeChunk(sound.soundwave);
            }
            soundLibrary.Clear();
            SDL_mixer.Mix_CloseAudio();
        }

        public void stopSound(string soundName)
        {
            if (occupiedChannels.TryGetValue(soundName, out int channel))
            {
                SDL_mixer.Mix_HaltChannel(channel);
                occupiedChannels.Remove(soundName); // Remove from tracking
                Debug.getInstance().log("Stopped: " + soundName);

            }
        }
    }
}
