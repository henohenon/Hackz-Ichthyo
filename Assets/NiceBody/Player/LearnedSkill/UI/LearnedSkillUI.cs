using UnityEngine;
using UnityEngine.UI;

namespace Player.Skill
{
    public sealed class LearnedSkillUI : MonoBehaviour
    {
        [SerializeField] Image icon_;

        public Image Icon => icon_;
    }
}
