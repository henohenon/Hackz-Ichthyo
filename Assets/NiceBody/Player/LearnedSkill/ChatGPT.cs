using System;
using Player.Skill;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Skill/ChatGPT")]
public sealed class ChatGPT : SkillBase
{
    [Header("HexAOE 設定")]
    [SerializeField] private HexAOE hexAOEPrefab;

    private ChatGptGradeProp[] grades =
    {
        // Lv1: 基本形。クールダウンは長いが、一度だけ確実なスペースを作る。
        new (2.0f, 5, 6.0f, -8f, 1),
        // Lv2: 発生数UP。一回の発動で、より広い範囲の敵を押しのけられるようになる。
        new (2.0f, 8, 5.5f, -10f, 2),
        // Lv3: 吹き飛ばし力UP。敵をより遠くへ押しやれるようになり、安全性が増す。
        new (2.2f, 8, 5.5f, -15f, 2),
        // Lv4: 発生数と範囲UP。複数の衝撃波で周囲に安全地帯を作りやすくなる。
        new (2.5f, 12, 5.0f, -18f, 3),
        // Lv5: ★★★質的変化★★★ クールダウンが大幅短縮。緊急回避だけでなく、牽制としても使えるように。
        new (2.5f, 15, 4.0f, -20f, 3),
        // Lv6: 火力と発生数UP。ダメージソースとしても無視できない存在になってくる。
        new (2.8f, 20, 4.0f, -22f, 4),
        // Lv7: 総合力UP。ほぼ常時、敵を寄せ付けないバリアのように機能し始める。
        new (3.0f, 25, 3.0f, -25f, 5),
        // Lv8 (MAX): 完成形。圧倒的な吹き飛ばし性能と回転率で、敵の接近を許さない。
        new (3.2f, 30, 2.5f, -30f, 6),
    };

    public override void OnAction(UseSkillContext context, int level)
    {
        if (level - 1 < 0) return;
        if (level - 1 >= grades.Length) level = grades.Length;
        var prop = grades[level - 1];
        cooldown_secs_ = prop.CoolDown;
        CreateHexField(context.Player.transform.position, prop);
    }

    private void CreateHexField(Vector3 center, ChatGptGradeProp props)
    {
        for (var i = 0; i < props.Count; i++)
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

[Serializable]
public sealed class ChatGptGradeProp
{
    [SerializeField]
    private float size;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float coolDown;
    [SerializeField]
    private float attractionForce;
    [SerializeField] private int count;

    public ChatGptGradeProp(float size, float damage, float coolDown, float attractionForce, int count)
    {
        this.size = size;
        this.damage = damage;
        this.coolDown = coolDown;
        this.attractionForce = attractionForce;
        this.count = count;
    }

    public float Size => size;
    public float Damage => damage;
    public float CoolDown => coolDown;
    public float AttractionForce => attractionForce;
    public int Count => count;
}