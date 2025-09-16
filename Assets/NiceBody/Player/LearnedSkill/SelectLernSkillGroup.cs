using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Skill
{
    [Serializable]
    public sealed class SelectLernSkillGroup
    {
        [SerializeField] private SkillBase[] learnSkills_ = new SkillBase[3];

        public IEnumerable<SkillBase> LearnSkills => learnSkills_;
    }
}
