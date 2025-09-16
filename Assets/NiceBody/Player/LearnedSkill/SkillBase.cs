using UnityEngine;

namespace Player.Skill
{
    public abstract class SkillBase : ScriptableObject
    {
        [SerializeField] private Sprite icon_;
        [SerializeField] private string skillName_;
        [SerializeField] private string skillDescription_;
        [SerializeField] private float cooldown_secs_;

        public Sprite Icon => icon_;
        public string SkillName => skillName_;
        public string SkillDescription => skillDescription_;
        public float Cooldown_secs_ => cooldown_secs_;  


        public abstract void OnActionLevel1(UseSkillContext context);
        public abstract void OnActionLevel2(UseSkillContext context);
        public abstract void OnActionLevel3(UseSkillContext context);

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