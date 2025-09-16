using R3;
using System;
using System.Collections.Generic;

namespace Player.Skill
{
    [Serializable]
    public class LearnedSkillGroup
    {
        private readonly List<LearnedSkill> learnedSkills_ = new();
        private readonly Subject<Unit> onLearnSkill_ = new();

        public Observable<Unit> OnLearnSkill => onLearnSkill_;

        public void LearnSkill(SkillBase skill, int initialLevel = 1)
        {
            if (skill == null) return;

            var existing = learnedSkills_.Find(ls => ls.Skill == skill);
            if (existing == null)
            {
                learnedSkills_.Add(new LearnedSkill(skill, initialLevel));
            }
            else
            {
                existing.LevelUp();
            }

            onLearnSkill_.OnNext(Unit.Default);
        }
    }
}