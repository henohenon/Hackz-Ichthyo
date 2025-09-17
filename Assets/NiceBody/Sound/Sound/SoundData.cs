using UnityEngine;

namespace GameCore.Asset
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "GameCore/SoundData")]
    public sealed class SoundData : ScriptableObject
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;

        public AudioClip AudioClip => audioClip;    
        public float Volume => volume;
    }
}
