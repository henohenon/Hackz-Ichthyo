using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Skill/ClaudeCode")]
    public class ClaudeCode: SkillBase
    {
        [Header("HexAOE 設定")]
        [SerializeField] private DrawingDamage drawingDamagePrefab;

        [SerializeField] private ClaudeGradeProps[] grades =
        {
            new (1, 1, 0.1f, 3, 2, 5, 2f),
            new (2, 1.5f, 0.13f, 6, 1, 8, 1f),
            new (3, 2.3f, 0.16f, 10, 0.5f, 12, 0.3f),
        };
    
        public override void OnAction(UseSkillContext context, int level)
        {
            if (level - 1 < 0) return;
            var prop = grades[level-1];
            cooldown_secs_ = prop.CoolDown;
            CreateHexField(context.Player.transform.position, context.Player.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-45, 45)), prop);
        }

        private void CreateHexField(Vector3 center, Quaternion rotate, ClaudeGradeProps props) {
            if (drawingDamagePrefab == null)
            {
                Debug.LogError("drawingDamagePrefab is not assigned.");
                return;
            }

            var instance = Instantiate(drawingDamagePrefab, center, rotate);
            instance.Initialize(props.Size, props.Damage, props.MoveSpeed, props.LifeTime, props.AttractionForce, props.BombCount);

        }
        
    }
    [Serializable]
    public class ClaudeGradeProps
    {
        [SerializeField]
        private float size;
        [SerializeField]
        private float damage;
        [SerializeField]
        private float attractionForce;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private int bombCount;
        [SerializeField]
        private float coolDown;

        public ClaudeGradeProps(float size, float damage, float attractionForce, float moveSpeed, float lifeTime,
            int bombCount, float coolDown)
        {
            this.size = size;
            this.damage = damage; 
            this.attractionForce = attractionForce;
            this.moveSpeed = moveSpeed;
            this.lifeTime = lifeTime;
            this.bombCount = bombCount;
            this.coolDown = coolDown;

        }
        
        public float Size => size;
        public float Damage => damage;
        public float AttractionForce => attractionForce;
        public float MoveSpeed => moveSpeed;
        public float LifeTime => lifeTime;
        public int BombCount => bombCount;
        public float CoolDown => coolDown;
    }
}
