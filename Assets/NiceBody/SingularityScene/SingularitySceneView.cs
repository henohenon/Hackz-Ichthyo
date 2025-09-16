using Player.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class SingularitySceneView
{
    [SerializeField] private Canvas onLearnSkillCanvas;
    [SerializeField] List<SelectLearnSkillUI> selectLearnSkillUIs;
    [SerializeField] private Text needCalculateTime;
    [SerializeField] Slider healthSlider_;

    public Text NeedCalculateTime => needCalculateTime;
    public Slider HealthSlider => healthSlider_;

    public IReadOnlyCollection<SelectLearnSkillUI> SelectLearnSkillUIs => selectLearnSkillUIs;


    public void OnOpenSelectLearnSkill(SelectLernSkillGroup skillGroup)
    {
        onLearnSkillCanvas.enabled = true;

        // Zip で SkillBase と UI を結合して設定
        skillGroup.LearnSkills
            .Zip(SelectLearnSkillUIs, (skill, ui) => (skill, ui))
            .ToList()
            .ForEach(pair => pair.ui.SetSkillInfo(pair.skill));
    }

    public void OnCloseSelectLearnSkill()
    {
        UnityEngine.Debug.Log("close canvas");
        onLearnSkillCanvas.enabled = false;
    }
}
