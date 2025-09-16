using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave")]
public sealed class WaveData : ScriptableObject
{
    [SerializeField] private float cooldown_secs;
    [SerializeField] List<SpawnEnemyData> summonEnemies;

    public IReadOnlyCollection<SpawnEnemyData> SummonEnemies => summonEnemies;
    public float CooldownSeconds => cooldown_secs;

    [Serializable]
    public sealed class SpawnEnemyData
    {
        [SerializeField] GameObject gameObject_;
        [SerializeField] private int count_;

        public GameObject GameObject_ => gameObject_;
        public int Count_ => count_;
    }
}