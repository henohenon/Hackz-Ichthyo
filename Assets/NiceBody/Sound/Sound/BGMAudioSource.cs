using GameCore.Asset;
using System;
using UnityEngine;

namespace GameUnity
{
    [Serializable]
    public sealed class BGMAudioSource
    {
        [SerializeField] private AudioSource audioSource_;
        [SerializeField] private float bgmMasterVolume;


        public void Play(SoundData bgmData, float masterVolume, bool loop = true)
        {
            audioSource_.clip = bgmData.AudioClip;
            audioSource_.volume = bgmData.Volume * bgmMasterVolume * masterVolume;
            audioSource_.loop = loop;
            audioSource_.Play();
        }
    }
}
