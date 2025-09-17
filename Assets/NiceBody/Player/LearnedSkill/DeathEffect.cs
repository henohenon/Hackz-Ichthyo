using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NiceBody.Player.LearnedSkill
{
    public class DeathEffect: MonoBehaviour
    {
        [SerializeField]
        private TextMesh text;
        public void Initialize(float iq)
        {
            text.text = $"完全に理解した！\n+{iq}IQ！";
            WaitAndDestroy().Forget();
        }

        private async UniTaskVoid WaitAndDestroy()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            Destroy(gameObject);
        }
    }
}