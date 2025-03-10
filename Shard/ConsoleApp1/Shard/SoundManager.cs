using SDL2;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    internal class SoundManager
    {
        private static SoundManager me;
        private Dictionary<string, IntPtr> soundLibrary;

        private Dictionary<string, int> occupiedChannels; // Store sound -> channel mapping

        private SoundManager()
        {
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
            {
                Console.WriteLine($"SDL_mixer could not initialize! SDL_mixer Error: {SDL.SDL_GetError()}");
            }

            soundLibrary = new Dictionary<string, IntPtr>();
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
            if (soundLibrary.TryGetValue(soundName, out IntPtr value))
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

            IntPtr sound = SDL_mixer.Mix_LoadWAV(filePath);
            if (sound == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load sound: {filePath}, SDL_mixer Error: {SDL.SDL_GetError()}");
                return;
            }

            soundLibrary[soundName] = sound;
            Console.WriteLine($"Sound {soundName} loaded successfully!");
        }

        public async Task playSoundWithDelay(string soundName, int delay, bool loop = false, int time = 0)
        {
            await Task.Delay(delay); // Ensure the delay is awaited properly
            playSound(soundName, loop, time);
        }

        public int playSound(string soundName, bool loop = false, int time = 0)
        {
            // sound is already playing. Skip.
            if (occupiedChannels.ContainsKey(soundName))
            {
                return -1;
            } else
            {
                if(soundLibrary.TryGetValue(soundName, out IntPtr sound)){
                    if (sound == IntPtr.Zero)
                    {
                        return -1;
                    }
                    int loops = loop ? -1 : 0;
                    int channel = SDL_mixer.Mix_PlayChannel(-1, sound, loops);
                    if (channel != -1)
                    {
                        occupiedChannels[soundName] = channel; // Store channel for this sound

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
            foreach (var (name, sound) in soundLibrary)
            {
                if (name == soundName)
                {
                    int sdlVolume = (int)(volume * 128);
                    SDL_mixer.Mix_VolumeChunk(sound, sdlVolume);
                    return;
                }
            }
        }

        public void dispose()
        {
            foreach (var (_, sound) in soundLibrary)
            {
                SDL_mixer.Mix_FreeChunk(sound);
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
            }
        }
    }


}
