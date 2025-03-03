using System;
using System.Collections.Generic;
using Shard.Shard;

namespace Shard.Shard.Components
{
    internal class SoundComponent : BaseComponent
    {
        private static Dictionary<string, string> loadedSounds = new Dictionary<string, string>();
        private string currentSound;
        private bool loop;

        public SoundComponent(GameObject owner) : base(owner)
        {
            // Empty constructor for adding multiple sounds later
        }

        public void loadSound(string soundName, string filePath)
        {
            if (!loadedSounds.ContainsKey(soundName))
            {
                loadedSounds[soundName] = filePath;
                SoundManager.getInstance().loadSound(soundName, filePath);
            }
        }

        public void playSound(string soundName, bool loop = false)
        {
            if (loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().playSound(soundName, loop);
                currentSound = soundName;
            }
            else
            {
                Console.WriteLine($"Sound {soundName} not found in loaded sounds.");
            }
        }

        public void playSoundOnRepeat(string soundName)
        {
            playSound(soundName, true);
        }

        public void stopSound(string soundName)
        {
            if (loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().stopSound(soundName);
            }
        }

        public void stopAllSounds()
        {
            SoundManager.getInstance().stopAllSounds();
        }

        public void setVolume(string soundName, float volume)
        {
            if (loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().setVolume(soundName, volume);
            }
        }
    }
}
