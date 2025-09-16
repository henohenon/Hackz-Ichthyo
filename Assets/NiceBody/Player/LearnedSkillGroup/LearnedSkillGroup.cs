using Alchemy.Inspector;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Player.Skill
{
    [Serializable]
    public sealed class LearnedSkillGroup
    {
        [SerializeField] private List<LearnedSkill> learnedSkills_ = new();
        private readonly Subject<SelectLernSkillGroup> onSelectLearnSkill = new();

        public Observable<SelectLernSkillGroup> OnSelectLearnSkill => onSelectLearnSkill;

        private readonly CancellationTokenSource skillLoopCts_ = new();

        public void SelectLearnSkill(SelectLernSkillGroup skill)
        {
            if (skill == null)
                return;

            onSelectLearnSkill.OnNext(skill);
        }

        public void LearnSkill(SkillBase skill)
        {
            if (skill == null)
                return;

            var existing = learnedSkills_.Find(ls => ls.Skill == skill);
            if (existing == null)
            {
                var newSkill = new LearnedSkill(skill, 1);

                var context = new SkillBase.UseSkillContext(UnityEngine.Object.FindObjectOfType<Player>());
                Cysharp.Threading.Tasks.UniTaskVoid uniTaskVoid = newSkill.LoopUseSkillAsync(context, skillLoopCts_.Token);

                learnedSkills_.Add(newSkill);
            }
            else
            {
                existing.LevelUp();
            }
        }

#if UNITY_EDITOR
        [SerializeField] private SelectLernSkillGroup selectSkillGroup;

        [Button]
        private void SelectLearnSkillButton()
        {
            SelectLearnSkill(selectSkillGroup);
        }
#endif
    }
}