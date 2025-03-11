﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shard.Shard;

namespace Shard.Shard.Components
{
    internal class SoundComponent : BaseComponent
    {
        private static Dictionary<string, string> loadedSounds = new Dictionary<string, string>();
        private string currentSound;
        private bool loop;

        public List<string> GetLoadedSoundNames()
        {
            return new List<string>(loadedSounds.Keys);
        }

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

        public void playSoundOnRepeat(string soundName)
        {
            playSound(soundName, true);
        }

        public void playSoundForSomeTime(string soundName, int time)
        {
            bool loop = false;
            if (loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().playSound(soundName, loop, time);
                currentSound = soundName;
            }
            else
            {
                Console.WriteLine($"Sound {soundName} not found in loaded sounds.");
            }
        }

        public Task playSoundWithDelay(string soundName, int delay)
        {
            return SoundManager.getInstance().playSoundWithDelay(soundName, delay);
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

        public void stopSound(string soundName)
        {
            if (loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().stopSound(soundName);
            }
        }

        public void stopAllComponentSounds(List<string> soundsNameList)
        {
<<<<<<< Updated upstream
            SoundManager.getInstance().stopAllSounds();
=======
            foreach (var soundName in soundsNameList)
            {
                if (loadedSounds.ContainsKey(soundName))
                {
                    SoundManager.getInstance().stopSound(soundName);
                    Debug.Log("Stopping sound: " + soundName);
                }
            }
>>>>>>> Stashed changes
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
