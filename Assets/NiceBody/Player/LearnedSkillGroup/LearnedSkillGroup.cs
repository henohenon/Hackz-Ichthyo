using Alchemy.Inspector;
using R3;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using NiceBody.Player.LearnedSkill;
using UnityEngine;

namespace Player.Skill
{
    [Serializable]
    public sealed class LearnedSkillGroup
    {
        [SerializeField] private List<LearnedSkill> learnedSkills_ = new();

        private readonly Subject<SelectLernSkillGroup> onSelectLearnSkill = new();
        public Observable<SelectLernSkillGroup> OnSelectLearnSkill => onSelectLearnSkill;

        private readonly Subject<LearnedSkill> onLearnSkill = new();
        public Observable<LearnedSkill> OnLearnSkill => onLearnSkill;

        private readonly CancellationTokenSource skillLoopCts_ = new();

        public void Init(Player player)
        {
            foreach (var learnedSkill in learnedSkills_)
            {
                var context = new SkillBase.UseSkillContext(player);
                learnedSkill.LoopUseSkillAsync(context, skillLoopCts_.Token).Forget();
            }
        }

        public void SelectLearnSkill(SelectLernSkillGroup skill)
        {
            if (skill == null)
                return;

            Time.timeScale = 0;
            onSelectLearnSkill.OnNext(skill);
        }

        public void LearnSkill(SkillBase skill)
        {
            Time.timeScale = 1;
            if (skill == null)
                return;

            var existing = learnedSkills_.Find(ls => ls.Skill == skill);
            if (existing == null)
            {
                var newSkill = new LearnedSkill(skill, 1);

                var context = new SkillBase.UseSkillContext(UnityEngine.Object.FindObjectOfType<Player>());
                UniTaskVoid uniTaskVoid = newSkill.LoopUseSkillAsync(context, skillLoopCts_.Token);

                learnedSkills_.Add(newSkill);

                // 新規習得を通知
                onLearnSkill.OnNext(newSkill);
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