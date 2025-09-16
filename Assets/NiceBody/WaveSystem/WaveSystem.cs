using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using System;
using System.Collections.Generic;

public sealed class WaveSystem : MonoBehaviour
{
    [SerializeField] private Player.Player player_;
    [SerializeField] private float spawnEnemyDistsanceRange_ = 1f;
    [SerializeField] private List<WaveData> waves_;
    [SerializeField] private List<EventBase> waveEvents_;

    private void Start()
    {
        RunWavesAsync().Forget();
        RunEventsAsync().Forget();
    }

    private async UniTaskVoid RunWavesAsync()
    {
        foreach (var wave in waves_)
        {
            Debug.Log($"Wave Start: {wave.name}");

            foreach (var enemyData in wave.SummonEnemies)
            {
                await SpawnEnemiesAsync(enemyData.GameObject_, enemyData.Count_);
            }

            Debug.Log($"Wave End: {wave.name}");
            await UniTask.Delay(TimeSpan.FromSeconds(wave.CooldownSeconds));
        }

        Debug.Log("All waves completed!");
    }

    private async UniTaskVoid RunEventsAsync()
    {
        foreach (var evt in waveEvents_)
        {
            await evt.ExecuteAsync(this);
        }
    }

    private async UniTask SpawnEnemiesAsync(GameObject prefab, int count)
    {
        NativeArray<Vector3> positions = new(count, Allocator.TempJob);
        NativeArray<uint> randomSeeds = new(count, Allocator.TempJob);

        // シード生成（適度に分散させる）
        uint baseSeed = (uint)UnityEngine.Random.Range(1, int.MaxValue);
        for (int i = 0; i < count; i++)
        {
            randomSeeds[i] = baseSeed + (uint)(i * 997); // 997 は素数で分散性を高める
        }

        var job = new EnemySpawnPositionJob
        {
            spawnRange = spawnEnemyDistsanceRange_,
            xRange = new Vector2(-5f, 5f),
            yRange = new Vector2(-3f, 3f),
            playerPosition = player_.transform.position,
            spawnPositions = positions,
            randomSeeds = randomSeeds
        };

        JobHandle handle = job.Schedule(count, 32);
        await UniTask.WaitUntil(() => handle.IsCompleted);
        handle.Complete();

        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(prefab, positions[i], Quaternion.identity);
            enemy.GetComponent<EnemyBase>().Initialize(player_);
        }

        positions.Dispose();
        randomSeeds.Dispose();

        Debug.Log($"Spawned {count} of {prefab.name}");
    }

    [BurstCompile]
    private struct EnemySpawnPositionJob : IJobParallelFor
    {
        [ReadOnly] public float spawnRange;
        [ReadOnly] public Vector2 xRange;
        [ReadOnly] public Vector2 yRange;
        [ReadOnly] public Vector3 playerPosition;

        [WriteOnly] public NativeArray<Vector3> spawnPositions;

        [ReadOnly] public NativeArray<uint> randomSeeds;

        public void Execute(int index)
        {
            var rand = new Unity.Mathematics.Random(randomSeeds[index]);

            float x = rand.NextFloat(xRange.x, xRange.y);
            float y = rand.NextFloat(yRange.x, yRange.y);

            Vector3 offset = new(x * spawnRange, y * spawnRange, 0f);
            spawnPositions[index] = playerPosition + offset;
        }
    }
}