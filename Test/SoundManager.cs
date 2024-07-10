

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Test
{

    public class SoundManager
    {
        private Dictionary<string, SoundEffect> SoundDict { get; }
        public Dictionary<string, SoundEffectInstance> CurrentSounds { get; }
        public Dictionary<string, SoundEffectInstance> SfxDictionary { get; }

        public float mSfxVolume;
        public float mMusicVolume;

        /// <summary>
        /// Contains current looped Sounds.
        /// -> Played Sound has to be looped = true
        /// </summary>


        /// <summary>
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public SoundManager()
        {
            SoundDict = ContentDictionary.SoundDict;
            CurrentSounds = new Dictionary<string, SoundEffectInstance>();
            SfxDictionary = new Dictionary<string, SoundEffectInstance>();
            mSfxVolume = 0.5f;
            mMusicVolume = 0.5f;
        }


        /// <summary>
        /// Plays a sound effect.
        /// If is looped is true. The Sound will be in the current Soundlist
        /// when played
        /// </summary>
        /// <param name="soundName">The name of the sound effect to play.</param>
        /// <param name="isLooped">Whether the sound should be looped.</param>
        public void PlaySound(string soundName, bool islooped)
        {
            if (!islooped)
            {
                SoundEffectInstance soundInstance = SoundDict[soundName].CreateInstance();
                soundInstance.Volume = MathHelper.Clamp(mSfxVolume, 0.0f, 1.0f);
                soundInstance.Play();
            }
            else
            {
                SoundEffectInstance soundInstance = SoundDict[soundName].CreateInstance();
                soundInstance.Volume = MathHelper.Clamp(mMusicVolume, 0.0f, 1.0f);
                soundInstance.IsLooped = true;

                if (!CurrentSounds.ContainsKey(soundName))
                {
                    CurrentSounds.Add(soundName, soundInstance);
                    soundInstance.Play();
                }
                else if (CurrentSounds[soundName].State == SoundState.Paused)
                {
                    CurrentSounds[soundName].Resume();
                }
            }
        }

        /// <summary>
        /// Stops a looped sound effect.
        /// </summary>
        public void StopSound(string soundName)
        {
            if (CurrentSounds.ContainsKey(soundName))
            {
                CurrentSounds[soundName].Stop();
                CurrentSounds.Remove(soundName);
            }
        }

        public void StopAll()
        {
            foreach (string playedSound in CurrentSounds.Keys)
            {
                StopSound(playedSound);
            }

            foreach (string playedSound in SfxDictionary.Keys)
            {
                StopSfx(playedSound);
            }
        }

        /// <summary>
        /// Pauses a looped sound effect.
        /// </summary>
        public void PauseSound(string soundName)
        {
            if (CurrentSounds.ContainsKey(soundName))
            {
                CurrentSounds[soundName].Pause();
            }
        }
        /// <summary>
        /// Resumes a paused looped sound effect.
        /// </summary>
        public void ResumeSound(string soundName)
        {
            if (CurrentSounds.ContainsKey(soundName))
            {
                if (CurrentSounds[soundName].State == SoundState.Paused)
                {
                    CurrentSounds[soundName].Resume();
                }
            }
        }

        /// <summary>
        /// Changes the volume of sound effects.
        /// </summary>
        /// <param name="newVolume">The new volume level for sound effects (0.0 to 1.0).</param>    
        public void ChangeSfxVolume(float newVolume)
        {
            mSfxVolume = MathHelper.Clamp(newVolume, 0.0f, 1.0f);

            foreach (var soundInstance in CurrentSounds.Values)
            {
                if (!soundInstance.IsLooped)
                {
                    soundInstance.Volume = mSfxVolume;
                }
            }
        }

        /// <summary>
        /// Changes the volume of background music.
        /// </summary>
        /// <param name="newVolume">The new volume level for music (0.0 to 1.0).</param>    
        public void ChangeMusicVolume(float newVolume)
        {
            mMusicVolume = MathHelper.Clamp(newVolume, 0.0f, 1.0f);

            //SoundEffect.MasterVolume = musicVolume;

            foreach (var soundInstance in CurrentSounds.Values)
            {
                if (soundInstance.IsLooped)
                {
                    soundInstance.Volume = mMusicVolume;
                }
            }
        }

        // these are Temporary Methods just for the Presentation 

        /// <summary>
        /// Plays a non-looped sound effect with the specified name.
        /// </summary>
        /// <param name="soundName">The name of the sound effect to play.</param>
        public void PlaySfx(string soundName)
        {
            SoundEffectInstance soundInstance = SoundDict[soundName].CreateInstance();
            soundInstance.Volume = MathHelper.Clamp(mSfxVolume, 0.0f, 1.0f);
            // soundInstance.Play();

            // // Add the non-looped sound to the sfxDictionary
            // SfxDictionary[soundName] = soundInstance;

            if (SfxDictionary.ContainsKey(soundName))
            {
                StopSfx(soundName);
                soundInstance.Play();
                SfxDictionary[soundName] = soundInstance;
            }
            else
            {
                soundInstance.Play();
                SfxDictionary[soundName] = soundInstance;
            }
        }

        /// <summary>
        /// Stops the currently playing non-looped sound effect with the specified name.
        /// </summary>
        /// <param name="soundName">The name of the sound effect to stop.</param>
        public void StopSfx(string soundName)
        {
            // Check if the non-looped sound is currently playing
            if (SfxDictionary.ContainsKey(soundName))
            {
                SfxDictionary[soundName].Stop();
                SfxDictionary.Remove(soundName);
            }
        }


        /// <summary>
        /// Clears the Sfxdict so all Sfx stop playing.
        /// </summary>
        public void ClearSfxSounds()
        {
            foreach (string key in SfxDictionary.Keys)
            {
                if (SfxDictionary[key].State == SoundState.Stopped)
                {
                    SfxDictionary.Remove(key);
                }
            }
        }


        /// <summary>
        /// Plays Sfx only when others Sfx are not playing.
        /// </summary>
        public void PlaySfxChecked(string sfxName)
        {
            SoundEffectInstance soundInstance = SoundDict[sfxName].CreateInstance();
            soundInstance.Volume = MathHelper.Clamp(mSfxVolume, 0.0f, 1.0f);
            if (!SfxDictionary.ContainsKey(sfxName))
            {
                SfxDictionary[sfxName] = soundInstance;
            }

            if (SfxDictionary[sfxName].State == SoundState.Stopped)
            { soundInstance.Play(); }

            ClearSfxSounds();
        }

        /// <summary>
        /// This is here because Sounds wont stop for some reason
        /// </summary>
        public void StopAllGameSounds()
        {
            StopSound("forestBiomAmbience.wav");
            StopSound("iceBiomAmbience.wav");
            StopSound("caveBiomAmbience.wav");
            StopSound("churchAmbience.wav");
            StopSound("villageAmbience.wav");
            StopSound("ForestEndBoss.wav");
            StopSound("IceEndboss.wav");
            StopSound("FireEndboss.wav");
            StopSound("Footsteps.wav");
            StopSound("goofy.wav");
            StopSfx("forgeAmbience.wav");
            StopSfx("tavernAmbience.wav");
            StopSfx("churchAmbience.wav");
            StopSfx("sheepsAmbience.wav");
            StopSfx("Enemy Walk Sound 1.wav");
            StopSfx("Enemy Walk Sound 2.wav");
            StopSfx("levelUpSoundeffect.wav");
            StopSfx("portalIdleSound.wav");
            StopSfx("slenderman.wav");
        }
    }

}


