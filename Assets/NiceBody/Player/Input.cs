using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public sealed class Input
    {
        [SerializeField] InputActionReference move_;
        [SerializeField] InputActionReference attack_;

        public InputActionReference Move => move_;
        public InputActionReference Attack => attack_;

        public void Init()
        {
            move_.action.Enable();
            attack_.action.Enable();
        }
    }
}
