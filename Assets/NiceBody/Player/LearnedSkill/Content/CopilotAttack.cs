using Player.State;
using UnityEngine;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Skill/CopilotAttack")]
    public sealed class Cursor : SkillBase
    {
        private readonly CopilotAttackGradeProps[] gradeProps_ =
        {
            new(2),
            new(4),
            new(7),
            new(13),
            new(20)
        };

        public override void OnAction(UseSkillContext context, int level)
        {
            context.Player.AttackPower_ = gradeProps_[level - 1].AttackPower;
        }
    }

    public sealed class CopilotAttackGradeProps
    {
        private readonly int attackPower_;

        public CopilotAttackGradeProps(int attackPower)
        {
            attackPower_ = attackPower;
        }

        public int AttackPower => attackPower_;
    }
}