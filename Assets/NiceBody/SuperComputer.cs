using R3;
using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public sealed class SuperComputer
    {
        [SerializeField] private float processingPower_;


        public void Tick(ReactiveProperty<IQ> iq)
        {
            iq.OnNext(new IQ(processingPower_ * Time.deltaTime) + iq.Value);
        }
    }
}
