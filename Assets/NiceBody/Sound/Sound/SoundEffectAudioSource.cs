using GameCore.Asset;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUnity
{
    [Serializable]
    public sealed class SoundEffectAudioSource
    {
        [SerializeField] private float masterVolume_;
        [SerializeField] private PlayAudioSourcesIndex currentPlayAudioSourceIndex;
        [SerializeField] private Transform groupTransform;
        [SerializeField, Range(0, 100)] private int maxConcurrentPlayLimit;

        private readonly List<AudioSource> audioSources_ = new();



        public void Initialize()
        {
            for(int i = 0; i < maxConcurrentPlayLimit; i++)
            {
                GameObject audioSourceObject = new("audioSource");
                audioSourceObject.transform.SetParent(groupTransform);
                audioSources_.Add(audioSourceObject.AddComponent<UnityEngine.AudioSource>());
            }
        }

        public void Play(SoundData soundEffectData, float masterVolume)
        {
            audioSources_[currentPlayAudioSourceIndex.Value].volume = soundEffectData.Volume * masterVolume_ * masterVolume;
            audioSources_[currentPlayAudioSourceIndex.Value].PlayOneShot(soundEffectData.AudioClip);
            currentPlayAudioSourceIndex.OnNext();
        }


        private sealed class PlayAudioSourcesIndex
        {
            int value = 0;
            const int MinValue = 0;
            readonly int maxValue;

            public int Value => value;


            public PlayAudioSourcesIndex(int maxValue)
            {
                this.maxValue = maxValue;
            }

            public void OnNext()
            {
                value++;
                if (Value > maxValue) value = MinValue;
            }
        }
    }
}