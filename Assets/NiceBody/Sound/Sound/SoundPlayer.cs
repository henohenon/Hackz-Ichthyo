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

#if UNITY_EDITOR
        [SerializeField] private SoundData test_bgmData;
        [SerializeField] private SoundData test_SoundEffectData;
#endif


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

        public void PlaySE(SoundData soundData)
        {
            soundEffectAudioSource_.Play(soundData, masterVolume_);
        }


#if UNITY_EDITOR
        public void PlayTestBGM()
        {
            PlayBGM(test_bgmData);
        }

        public void PlayTestSoundEffect()
        {
            PlaySE(test_SoundEffectData);
        }
#endif
    }
}