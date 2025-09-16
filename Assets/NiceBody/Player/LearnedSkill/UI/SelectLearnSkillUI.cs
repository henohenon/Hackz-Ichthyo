using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Player.Skill;
using R3;
using System;

[RequireComponent(typeof(Image))]
public sealed class SelectLearnSkillUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SkillBase skill_;
    [SerializeField] private Image icon_;
    [SerializeField] private Text name_;
    [SerializeField] private Text description_;

    private readonly Subject<SkillBase> onClickSubject_ = new();

    public SkillBase Skill => skill_;
    public Observable<SkillBase> OnClicked => onClickSubject_;

    public void SetSkillInfo(SkillBase skill)
    {
        skill_ = skill;
        if (skill == null) return;

        if (icon_ != null) icon_.sprite = skill.Icon;
        if (name_ != null) name_.text = skill.SkillName;
        if (description_ != null) description_.text = skill.SkillDescription;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (skill_ != null)
        {
            onClickSubject_.OnNext(skill_);
        }
    }
}