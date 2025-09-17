using UnityEngine;
using Player.Skill;
using System;

[CreateAssetMenu(menuName = "Skill/Gemini")]
public sealed class Gemini : SkillBase
{
    [Header("Geminiボール設定")]
    [SerializeField] private GeminiBullet projectilePrefab_;
    [SerializeField] private int damage_ = 3;
    [SerializeField] private float projectileSpeed_ = 5f;
    [SerializeField] private int projectileCount_ = 5;
    [SerializeField, Range(20f, 30f)] private float spreadAngle_ = 25f;

    public override void OnAction(UseSkillContext context, int level)
    {
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
        float startAngle = -spreadAngle_ / 2f;
        float angleStep = (projectileCount_ > 1) ? spreadAngle_ / (projectileCount_ - 1) : 0f;

        for (int i = 0; i < projectileCount_; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDeg) * Quaternion.Euler(0, 0, currentAngle);
            GeminiBullet geminiBullet = Instantiate(projectilePrefab_, spawnPosition, rotation);
            if (geminiBullet != null)
            {
                // レベルごとに処理を分けることもできる
                geminiBullet.Initialize(projectileSpeed_, damage_);
            }
        }
    }
}