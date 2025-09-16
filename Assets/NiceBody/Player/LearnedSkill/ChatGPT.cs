using Player.Skill;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/ChatGPT")]
public sealed class ChatGPT : SkillBase
{
    [Header("HexAOE 設定")]
    [SerializeField] private HexAOE hexAOEPrefab;
    [SerializeField] private float baseSize = 1f;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private int baseCount = 1;

    [SerializeField] private float level2Size = 1.5f;
    [SerializeField] private float level2Damage = 1.5f;
    [SerializeField] private int level2Count = 3;

    [SerializeField] private float level3Size = 2f;
    [SerializeField] private float level3Damage = 2f;
    [SerializeField] private int level3Count = 6;

    public override void OnActionLevel1(UseSkillContext context)
    {
        CreateHexField(context.Player.transform.position, baseSize, baseDamage, baseCount);
    }

    public override void OnActionLevel2(UseSkillContext context)
    {
        CreateHexField(context.Player.transform.position, level2Size, level2Damage, level2Count);
    }

    public override void OnActionLevel3(UseSkillContext context)
    {
        CreateHexField(context.Player.transform.position, level3Size, level3Damage, level3Count, attractEnemies: true);
    }

    private void CreateHexField(Vector3 center, float size, float damage, int count, bool attractEnemies = false)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = GetHexOffset(i, size);
            var aoe = InstantiateHexAOE(center + offset, size, damage);
            if (attractEnemies)
            {
                aoe.EnableAttraction();
            }
        }
    }

    private Vector3 GetHexOffset(int index, float radius)
    {
        float angle = index * 60f * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    }

    private HexAOE InstantiateHexAOE(Vector3 position, float size, float damage)
    {
        if (hexAOEPrefab == null)
        {
            Debug.LogError("HexAOEPrefab is not assigned.");
            return null;
        }

        var aoe = GameObject.Instantiate(hexAOEPrefab, position, Quaternion.identity);
        aoe.Initialize(size, damage);
        return aoe;
    }
}