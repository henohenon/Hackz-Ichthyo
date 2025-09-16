using UnityEngine;
using System;

namespace Player.Skill
{
    [Serializable]
    public sealed class LearnedSkill
    {
        [SerializeField] SkillBase skill;
        [SerializeField] int level;

        public LearnedSkill(SkillBase skill, int level)
        {
            this.skill = skill;
            this.level = level;
        }

        public SkillBase Skill => skill;
        public int Level => level;

        public void LevelUp()
        {
            level++;
        }
    }
}