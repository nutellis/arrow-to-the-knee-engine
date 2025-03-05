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
        private List<(string name, IntPtr sound)> soundLibrary; 

        private SoundManager()
        {
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
            {
                Console.WriteLine($"SDL_mixer could not initialize! SDL_mixer Error: {SDL.SDL_GetError()}");
            }

            soundLibrary = new List<(string, IntPtr)>();
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
            filePath = Bootstrap.getAssetManager().getAssetPath(filePath);
            Console.WriteLine($"Loading sound: {soundName} from {filePath}");

            if (!System.IO.File.Exists(filePath))  // Check if file exists
            {
                Console.WriteLine($"ERROR: File not found -> {filePath}");
                return;
            }

            IntPtr sound = SDL_mixer.Mix_LoadWAV(filePath);
            if (sound == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load sound: {filePath}, SDL_mixer Error: {SDL.SDL_GetError()}");
                return;
            }

            soundLibrary.Add((soundName, sound));
            Console.WriteLine($"Sound {soundName} loaded successfully!");
        }

        public void playSound(string soundName, bool loop = false, int time = 0)
        {
            var soundEntry = soundLibrary.FirstOrDefault(s => s.name == soundName);
            if (soundEntry.sound == IntPtr.Zero)
            {
                Console.WriteLine($"ERROR: Sound {soundName} not loaded!");
                return;
            }

            int loops = loop ? -1 : 0;
            int channel = SDL_mixer.Mix_PlayChannel(-1, soundEntry.sound, loops);

            if (channel == -1)
            {
                Console.WriteLine($"ERROR: Failed to play sound {soundName}, SDL_mixer Error: {SDL.SDL_GetError()}");
            }
            else
            {
                Console.WriteLine($"Playing sound {soundName} on channel {channel}");
                if (time > 0)
                {
                    Task.Delay(time).ContinueWith(_ => SDL_mixer.Mix_HaltChannel(channel));
                }
            }
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
            Console.WriteLine($"Sound {soundName} not found!");
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
            // Get the channel currently playing the sound
            int numChannels = SDL_mixer.Mix_AllocateChannels(-1);

            for (int i = 0; i < numChannels; i++)
            {
                if (SDL_mixer.Mix_Playing(i) != 0) // Check if a channel is playing
                {
                    SDL_mixer.Mix_HaltChannel(i);
                    Console.WriteLine($"Stopped sound {soundName} on channel {i}");
                    return;
                }
            }

            Console.WriteLine($"Sound {soundName} was not playing!");
        }
    }


}
