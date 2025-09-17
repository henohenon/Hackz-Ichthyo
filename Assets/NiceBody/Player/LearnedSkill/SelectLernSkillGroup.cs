using System;
using System.Collections.Generic;
using Player.Skill;
using UnityEngine;

namespace NiceBody.Player.LearnedSkill
{
    [Serializable]
    public sealed class SelectLernSkillGroup
    {
        [SerializeField] private SkillBase[] learnSkills_ = new SkillBase[3];

        public IEnumerable<SkillBase> LearnSkills => learnSkills_;
    }
}
