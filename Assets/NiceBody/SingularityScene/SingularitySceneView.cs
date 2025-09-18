using Player.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class SingularitySceneView
{
    [SerializeField] LearnedSkillUI learnedSkillUISlotPrefab;
    [SerializeField] RectTransform skillSlotGroup;
    [SerializeField] private Canvas onLearnSkillCanvas;
    [SerializeField] List<SelectLearnSkillUI> selectLearnSkillUIs;
    [SerializeField] private Text hourText;
    [SerializeField] private Text minuteText;
    [SerializeField] private Text secondsText;
    [SerializeField] private Transform sliderMask;
    [SerializeField] Slider healthSlider_;
    [SerializeField] Canvas deathCanvas_;
    [SerializeField] Canvas kakuseiCanvas_;

    public void SetSingularityTime(TimeSpan time, float progress)
    {
        hourText.text = time.Hours.ToString("00");
        minuteText.text = time.Minutes.ToString("00");
        secondsText.text = time.Seconds.ToString("00");
        
        sliderMask.localPosition = new Vector3(progress * 17.2f, 0, 0);
    }
    
    
    public Slider HealthSlider => healthSlider_;

    public IReadOnlyCollection<SelectLearnSkillUI> SelectLearnSkillUIs => selectLearnSkillUIs;


    public void OnOpenSelectLearnSkill(SelectLernSkillGroup skillGroup)
    {
        onLearnSkillCanvas.enabled = true;

        // Zip で SkillBase と UI を結合して設定
        skillGroup.SelectRandomSkills()
            .Zip(SelectLearnSkillUIs, (skill, ui) => (skill, ui))
            .ToList()
            .ForEach(pair => pair.ui.SetSkillInfo(pair.skill));
    }

    public void AddLearnSkill(SkillBase learnSkill) => UnityEngine.Object.Instantiate(learnedSkillUISlotPrefab, skillSlotGroup).Icon.sprite = learnSkill.Icon;
    public void OnCloseSelectLearnSkill()
    {
        if (!deathCanvas_.enabled && !kakuseiCanvas_.enabled) onLearnSkillCanvas.enabled   = false;
    }

    public void OnEnableDeathCanvas()
    {
        if (kakuseiCanvas_.enabled) return;
        deathCanvas_.enabled = true;
    }

    public void OnEnableKakuseiCanvas()
    {
        if (deathCanvas_.enabled) return;
        kakuseiCanvas_.enabled = true;
    }
}
