using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class SingularitySceneView
{
    [SerializeField] private Text needCalculateTime;

    public Text NeedCalculateTime => needCalculateTime;
}
