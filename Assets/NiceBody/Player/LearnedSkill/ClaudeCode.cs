using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Skill/ClaudeCode")]
    public class ClaudeCode : SkillBase
    {
        [Header("HexAOE 設定")]
        [SerializeField] private DrawingDamage drawingDamagePrefab;

        private ClaudeGradeProps[] grades =
        {
            // Lv1: 基本形。ささやかな防御兼足止めとして。
            new (1.5f, 4, 0.5f, 20f, 2.0f, 0, 5.0f),
            // Lv2: 純粋強化。範囲と持続時間を伸ばし、使いやすくする。
            new (1.8f, 6, 0.7f, 25f, 2.5f, 0, 4.8f),
            // Lv3: 吸引力の向上。初めて「敵を吸い寄せる」感覚が明確になる。
            new (2.0f, 8, 1.2f, 30f, 3.0f, 0, 4.5f),
            // Lv4: 火力と範囲の強化。ダメージ効率が上がり、メインウェポンと連携しやすくなる。
            new (2.5f, 12, 1.5f, 35f, 3.0f, 0, 4.5f),
            // Lv5: ★★★質的変化★★★ 効果終了時に爆発(Bomb)するようになり、攻撃性能が飛躍的に向上。
            new (2.8f, 15, 1.8f, 40f, 3.5f, 4, 4.0f),
            // Lv6: 爆発と吸引力の強化。より多くの敵を、より強く引き寄せ、より大きなダメージを与える。
            new (3.0f, 20, 2.2f, 45f, 4.0f, 6, 4.0f),
            // Lv7: 回転率の向上。クールダウンが大幅に短縮され、フィールドを常時展開しやすくなる。
            new (3.5f, 25, 2.5f, 50f, 4.5f, 6, 3.5f),
            // Lv8 (MAX): 完成形。「敵を吸い込み続ける破壊の渦」となり、プレイヤーに絶大な安心感を与える。
            new (4.0f, 35, 3.0f, 55f, 5.0f, 8, 3.0f),
        };

        public override void OnAction(UseSkillContext context, int level)
        {
            if (level - 1 < 0) return;
            if (level - 1 >= grades.Length) level = grades.Length;
            var prop = grades[level - 1];
            cooldown_secs_ = prop.CoolDown;
            CreateHexField(context.Player.transform.position, context.Player.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-45, 45)), prop);
        }

        private void CreateHexField(Vector3 center, Quaternion rotate, ClaudeGradeProps props)
        {
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
