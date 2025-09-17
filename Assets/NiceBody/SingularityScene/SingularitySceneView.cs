using Player.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using NiceBody.Player.LearnedSkill;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class SingularitySceneView
{
    [SerializeField] LearnedSkillUI learnedSkillUISlotPrefab;
    [SerializeField] RectTransform skillSlotGroup;
    [SerializeField] private Canvas onLearnSkillCanvas;
    [SerializeField] List<SelectLearnSkillUI> selectLearnSkillUIs;
    [SerializeField] private Text needCalculateTime;
    [SerializeField] Slider healthSlider_;
    [SerializeField] Canvas deathCanvas_;

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

    public void AddLearnSkill(SkillBase learnSkill) => UnityEngine.Object.Instantiate(learnedSkillUISlotPrefab, skillSlotGroup).Icon.sprite = learnSkill.Icon;
    public void OnCloseSelectLearnSkill()   => onLearnSkillCanvas.enabled   = false;
    public void OnEnableDeathCanvas()       =>  deathCanvas_.enabled        = true;
}
