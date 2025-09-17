using Player.Skill;
using UnityEngine;

namespace NiceBody.Player.LearnedSkill
{
    [CreateAssetMenu(menuName = "Skill/Gemini")]
    public class Gemini: SkillBase
    {

        [Header("HexAOE 設定")]
        [SerializeField] private HexAOE hexAOEPrefab;

        [SerializeField] private ChatGptGradeProp[] grades =
        {
            new ChatGptGradeProp(1, 1, 5, 1.5f, 1),
            new ChatGptGradeProp(2, 2, 3, 5, 2),
            new ChatGptGradeProp(3, 3, 1, 12, 3),
        };
    
        public override void OnAction(UseSkillContext context, int level)
        {
            if (level - 1 < 0) return;
            var prop = grades[level-1];
            cooldown_secs_ = prop.CoolDown;
            CreateHexField(context.Player.transform.position, prop);
        }

        private void CreateHexField(Vector3 center, ChatGptGradeProp props) {
            for(var i = 0; i < props.Count; i++)
            {
                var offset = GetHexOffset(4);
                InstantiateHexAOE(center + offset, props.Size, props.Damage, props.AttractionForce);
            }
        }

        private Vector3 GetHexOffset(float radius)
        {
            float randomX = Random.Range(-radius, radius);
            float randomY = Random.Range(-radius, radius);
            return new Vector3(randomX, randomY, 0);
        }

        private HexAOE InstantiateHexAOE(Vector3 position, float size, float damage, float attractionForce)
        {
            if (hexAOEPrefab == null)
            {
                Debug.LogError("HexAOEPrefab is not assigned.");
                return null;
            }

            var aoe = Instantiate(hexAOEPrefab, position, Quaternion.identity);
            aoe.Initialize(size, damage, attractionForce);
            return aoe;
        }        
    }
    
    public class GeminiGradeProps
    {
        public float Size;
        public float CoolDown;
        public int Count;
    }
}