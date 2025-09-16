using R3;
using System;
using System.Collections.Generic;

namespace Player.Skill
{
    [Serializable]
    public sealed class LearnedSkillGroup
    {
        private readonly List<LearnedSkill> learnedSkills_ = new();
        private readonly Subject<SelectLernSkillGroup> onSelectLearnSkill = new();

        public Observable<SelectLernSkillGroup> OnSelectLearnSkill => onSelectLearnSkill;

        public void SelectLearnSkill(SelectLernSkillGroup skill)
        {
            if (skill == null) 
                return;

            // Subject 経由で通知
            onSelectLearnSkill.OnNext(skill);
        }

        public void LearnSkill(SkillBase skill)
        {
            if (skill == null) 
                return;

            var existing = learnedSkills_.Find(ls => ls.Skill == skill);
            if (existing == null)
            {
                learnedSkills_.Add(new LearnedSkill(skill, 1));
            }
            else
            {
                existing.LevelUp();
            }
        }
    }
}