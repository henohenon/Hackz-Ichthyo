using UnityEngine;
using Player.Skill;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "Skill/Gemini")]
public sealed class Gemini : SkillBase
{
    [Header("Geminiボール設定")]
    [SerializeField] private GeminiBullet projectilePrefab_;
    [SerializeField]
    private GeminiGradeProps[] grades =
    {
        new (5, 4f, 2, 20f, 0.8f, 1.0f, 0,1.5f),
        // Lv2: 純粋強化。ダメージと弾数を増やす。
        new (8, 4f, 3, 25f, 0.8f, 1.0f, 0,1.4f),
        // Lv3: 安定性向上。弾が大きくなり、射程も少し伸びる。
        new (12, 4.5f, 3, 25f, 0.9f, 1.2f, 0,1.2f),
        // Lv4: 攻撃範囲の拡大。弾数と角度を広げて面を制圧しやすくする。
        new (15, 4.5f, 4, 30f, 1.0f, 1.2f, 0,1.2f),
        // Lv5: ★★★質的変化★★★ 弾数を増やし、爽快感が大きく向上する。
        new (18, 5f, 5, 35f, 1.1f, 1.3f, 1,1.2f),
        // Lv6: さらなる火力強化。
        new (25, 5f, 5, 35f, 1.1f, 1.4f, 1,1f),
        // Lv7: 弾幕形成。弾数と角度を増やして隙をなくす。
        new (30, 5.5f, 6, 40f, 1.2f, 1.4f, 1,0.8f),
        // Lv8 (MAX): 完成形。圧倒的な制圧力。
        new (40, 5.5f, 7, 45f, 1.2f, 1.5f, 2,0.8f),
    };

    public override int MaxLevel => grades.Length;

    public override void OnAction(UseSkillContext context, int level)
    {
        if (level - 1 < 0) return;
        if (level - 1 >= grades.Length) level = grades.Length;
        var prop = grades[level - 1];
        Transform playerTransform = context.Player.transform;
        if (projectilePrefab_ == null)
        {
            Debug.LogError("Projectile Prefabが設定されていません！");
            return;
        }
        Vector2 moveInput = context.Player.GetMoveInput();
        float angleRad = Mathf.Atan2(moveInput.y, moveInput.x); // ラジアンを取得
        float angleDeg = angleRad * Mathf.Rad2Deg; // 度数に変換


        Vector3 spawnPosition = playerTransform.position;
        float startAngle = -prop.SpreadAngle / 2f;
        float angleStep = (prop.ProjectileCount > 1) ? prop.SpreadAngle / (prop.ProjectileCount - 1) : 0f;

        for (int i = 0; i < prop.ProjectileCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDeg) * Quaternion.Euler(0, 0, currentAngle);
            GeminiBullet geminiBullet = Instantiate(projectilePrefab_, spawnPosition, rotation);
            if (geminiBullet != null)
            {
                geminiBullet.transform.localScale = Vector3.one * prop.Size;
                geminiBullet.Initialize(prop.ProjectileSpeed, prop.Damage, prop.LifeTime, prop.PierceCount);
            }
        }
    }
}

public class GeminiGradeProps
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float projectileSpeed_;
    [SerializeField]
    private int projectileCount_;
    [SerializeField, Range(20f, 30f)] private float spreadAngle_ = 25f;

    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float size;
    [SerializeField]
    private int pierceCount_;
    [SerializeField]
    private float cooldown_;

    public GeminiGradeProps(int damage, float projectileSpeed_, int projectileCount_, float spreadAngle_, float lifeTime, float size, int pierceCount, float cooldown)
    {
        this.damage = damage;
        this.projectileSpeed_ = projectileSpeed_;
        this.projectileCount_ = projectileCount_;
        this.spreadAngle_ = spreadAngle_;
        this.lifeTime = lifeTime;
        this.size = size;
        this.pierceCount_ = pierceCount;
        this.cooldown_ = cooldown;
    }

    public int Damage => damage;
    public float ProjectileSpeed => projectileSpeed_;
    public int ProjectileCount => projectileCount_;
    public float SpreadAngle => spreadAngle_;
    public float LifeTime => lifeTime;
    public float Size => size;
    public int PierceCount => pierceCount_;
    public float Cooldown => cooldown_;
}
