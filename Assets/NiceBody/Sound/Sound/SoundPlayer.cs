using System.Collections.Generic;
using GameCore.Asset;
using UnityEngine;

namespace GameUnity
{
    public sealed class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private BGMAudioSource bgmAudioSource_;
        [SerializeField] private SoundEffectAudioSource soundEffectAudioSource_;
        [SerializeField] private float masterVolume_;
        [SerializeField] private SoundData bgmData_;
        [SerializeField] private List<SoundData> soundEffectDataList;


        public void Awake()
        {
            new Sound(this);
            soundEffectAudioSource_.Initialize();
            PlayBGM(bgmData_);
        }

        public void PlayBGM(SoundData bgmData)
        {
            bgmAudioSource_.Play(bgmData, masterVolume_, true);
            UnityEngine.Debug.Log("jfoiwjifa");
        }

        public void PlaySE(SoundEffectType soundEffectType)
        {
            soundEffectAudioSource_.Play((int)soundEffectType, soundEffectDataList[(int)soundEffectType], masterVolume_);
        }
    }
}