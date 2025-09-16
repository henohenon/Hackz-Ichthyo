using UnityEngine;

namespace Player.Skill
{
    public abstract class SkillBase : ScriptableObject
    {
        public abstract void OnAction(UseSkillContext context);

        public readonly struct UseSkillContext
        {
            private readonly Player player_;

            public UseSkillContext(Player player)
            {
                player_ = player;
            }

            public Player Player => player_;
        }
    }
}
