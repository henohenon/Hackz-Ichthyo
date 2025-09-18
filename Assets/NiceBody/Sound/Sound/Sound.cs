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

        public static void PlaySE(SoundEffectType soundEffectType)
        {
            Debug.Log("SEを再生します: " + soundEffectType);
            soundPlayer.PlaySE(soundEffectType);
        }
    }
}