using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class SingularitySceneView
{
    [SerializeField] Canvas onLearnSkillCanvas;
    [SerializeField] private Text needCalculateTime;

    public Text NeedCalculateTime => needCalculateTime;
}
