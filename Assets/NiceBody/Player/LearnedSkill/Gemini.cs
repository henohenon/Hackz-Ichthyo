using UnityEngine;
using Player.Skill;
using System;

[CreateAssetMenu(menuName = "Skill/Gemini")]
public sealed class Gemini : SkillBase
{
    [Header("扇状ショット設定")]
    [SerializeField] private GameObject projectilePrefab_;
    [SerializeField]
    private int damage_ = 10;
    [SerializeField]
    private float projectileSpeed_ = 15f;
    [SerializeField]
    private int projectileCount_ = 5;
    [SerializeField]
    [Range(20f, 30f)] // インスペクターで角度を20~30度に制限
    private float spreadAngle_ = 25f;

    public override void OnAction(UseSkillContext context, int level)
    {
        Transform playerTransform = context.Player.transform;
        if (projectilePrefab_ == null)
        {
            Debug.LogError("Projectile Prefabが設定されていません！");
            return;
        }
        float startAngle = -spreadAngle_ / 2f;
        float angleStep = (projectileCount_ > 1) ? spreadAngle_ / (projectileCount_ - 1) : 0f;

        for (int i = 0; i < projectileCount_; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Quaternion rotation = playerTransform.rotation * Quaternion.Euler(0, currentAngle, 0);
            GameObject projectileObj = Instantiate(projectilePrefab_, playerTransform.position, rotation);
            GeminiBullet geminiBullet = projectileObj.GetComponent<GeminiBullet>();
            if (geminiBullet != null)
            {
                // レベルごとに処理を分けることもできる
                geminiBullet.Initialize(projectileSpeed_, damage_);
            }
        }
    }
}