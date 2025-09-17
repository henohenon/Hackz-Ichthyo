using System;
using Player.Skill;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Skill/ChatGPT")]
public sealed class ChatGPT : SkillBase
{
    [Header("HexAOE 設定")]
    [SerializeField] private HexAOE hexAOEPrefab;

    [SerializeField] private ChatGptGradeProp[] grades =
    {
        new ChatGptGradeProp(1, 1, 5, 1.5f),
        new ChatGptGradeProp(2, 2, 3, 5),
        new ChatGptGradeProp(3, 3, 1, 12),
    };
    
    public override void OnAction(UseSkillContext context, int level)
    {
        if (level - 1 < 0) return;
        var prop = grades[level-1];
        cooldown_secs_ = prop.CoolDown;
        CreateHexField(context.Player.transform.position, prop);
    }

    private void CreateHexField(Vector3 center, ChatGptGradeProp props)
    {
        var offset = GetHexOffset(4);
        InstantiateHexAOE(center + offset, props.Size, props.Damage, props.AttractionForce);
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

        var aoe = GameObject.Instantiate(hexAOEPrefab, position, Quaternion.identity);
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
    
    public ChatGptGradeProp(float size, float damage, float coolDown, float attractionForce)
    {
        this.size = size;
        this.damage = damage;
        this.coolDown = coolDown;
        this.attractionForce = attractionForce;
    }
    
    public float Size => size;
    public float Damage => damage;
    public float CoolDown => coolDown;
    public float AttractionForce => attractionForce;
}