using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/Wave")]
public sealed class WaveData : ScriptableObject
{
    [SerializeField] private float cooldown_secs;
    [SerializeField] private IQ requiredIQ_;
    [SerializeField] List<SpawnEnemyData> summonEnemies;

    public IReadOnlyCollection<SpawnEnemyData> SummonEnemies => summonEnemies;
    public IQ RequiredIQ => requiredIQ_;
    public float CooldownSeconds => cooldown_secs;


    [Serializable]
    public sealed class SpawnEnemyData
    {
        [SerializeField] private WaveSystem.EnemyType type;
        [SerializeField] private int count;
        
        public WaveSystem.EnemyType Type => type;
        public int Count => count;
    }
}