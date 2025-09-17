using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Player.Skill
{
    [Serializable]
    public sealed class LearnedSkill
    {
        [SerializeField] SkillBase skill;
        [SerializeField, Range(0, 3)] int level;

        public LearnedSkill(SkillBase skill, int level)
        {
            this.skill = skill;
            this.level = level;
        }

        public SkillBase Skill => skill;
        public int Level => level;

        public void LevelUp()
        {
            level = Mathf.Clamp(level + 1, 0, 3);
        }

        public async UniTaskVoid LoopUseSkillAsync(SkillBase.UseSkillContext context, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                skill.OnAction(context, level);
                await UniTask.Delay(TimeSpan.FromSeconds(skill.Cooldown_secs_), cancellationToken: token);
            }
        }
    }
}