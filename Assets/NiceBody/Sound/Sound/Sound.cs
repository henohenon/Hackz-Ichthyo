using UnityEngine;

namespace GameUnity
{
    /// <summary>
    /// SinglePlay用HelperClass
    /// </summary>
    public sealed class Sound
    {
        private static SoundPlayer soundPlayer = null;



        public Sound(SoundPlayer soundPlayer)
        {
            Sound.soundPlayer = soundPlayer;
        }

        public static void PlayBGM(GameCore.Asset.SoundData bgmData)
        {
            soundPlayer.PlayBGM(bgmData);
        }

        public static void PlaySE(GameCore.Asset.SoundData soundEffectData)
        {
            soundPlayer.PlaySE(soundEffectData);
        }


#if UNITY_EDITOR
        public static void PlayTestBGM()
        {
            soundPlayer.PlayTestBGM();
        }

        public static void PlayTestSoundEffect()
        {
            soundPlayer.PlayTestSoundEffect();
        }
#endif
    }
}