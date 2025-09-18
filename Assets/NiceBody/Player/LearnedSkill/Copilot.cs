using Player.State;
using UnityEngine;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Skill/Copilot")]
    public sealed class Copilot : SkillBase
    {
        private readonly CopilotGradeProps[] gradeProps_ =
        {
            new (3),
            new (4),
            new (5),
            new (6),
            new (7)
        };

        public override void OnAction(UseSkillContext context, int level)
        {
            context.Player.GetStateContext<WalkStateContext>().SetSpeed(gradeProps_[level - 1].Speed);
        }
    }

    public sealed class CopilotGradeProps
    {
        private readonly int speed_;

        public CopilotGradeProps(int speed)
        {
            speed_ = speed;
        }

        public int Speed => speed_;
    }
}
