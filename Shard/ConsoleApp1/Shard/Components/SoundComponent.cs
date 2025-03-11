using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shard.Shard;

namespace Shard.Shard.Components
{
    internal class SoundComponent : BaseComponent
    {
        private Dictionary<string, bool> loadedSounds;
        private string currentSound;
        private int soundChannel = -1;

        private bool loop;

        public SoundComponent(GameObject owner) : base(owner)
        {
            loadedSounds = new Dictionary<string, bool>();
        }

        public void loadSound(string soundName, string filePath)
        {
            if (!loadedSounds.ContainsKey(soundName))
            {
                SoundManager.getInstance().loadSound(soundName, filePath);
                
                loadedSounds[soundName] = false;
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
                SoundManager.getInstance().playSound(soundName, true, time);
                currentSound = soundName;
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
            }
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
            //TODO: @christos
            //stop all sounds coming from this component!
            //SoundManager.getInstance().stopAllSounds(List<string> sounds);

            foreach(string sound in loadedSounds.Keys)
            {
                SoundManager.getInstance().stopSound(sound);
                Debug.getInstance().log("Trying to Sound " + sound);
            }
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
