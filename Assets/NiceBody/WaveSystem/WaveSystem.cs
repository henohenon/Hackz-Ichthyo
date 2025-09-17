using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using System;
using System.Collections.Generic;
using System.Threading;

public sealed class WaveSystem : MonoBehaviour
{
    [SerializeField] private Player.Player player_;
    [SerializeField] private float spawnEnemyDistsanceRange_ = 1f;
    [SerializeField] private List<WaveData> waves_;
    [SerializeField] private List<EventBase> waveEvents_;

    private CancellationTokenSource cts_;

    private void Start()
    {
        cts_ = new CancellationTokenSource();
        RunWavesAsync(cts_.Token).Forget();
        RunEventsAsync(cts_.Token).Forget();
    }

    private void OnDestroy()
    {
        cts_?.Cancel();
        cts_?.Dispose();
    }

    private async UniTaskVoid RunWavesAsync(CancellationToken token)
    {
        try
        {
            foreach (var wave in waves_)
            {
                Debug.Log($"Wave Start: {wave.name}");

                foreach (var enemyData in wave.SummonEnemies)
                {
                    await SpawnEnemiesAsync(enemyData.GameObject_, enemyData.Count_, token);
                }

                Debug.Log($"Wave End: {wave.name}");
                await UniTask.Delay(TimeSpan.FromSeconds(wave.CooldownSeconds), cancellationToken: token);
            }

            Debug.Log("All waves completed!");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Wave execution canceled.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Wave execution error: {ex}");
        }
    }

    private async UniTaskVoid RunEventsAsync(CancellationToken token)
    {
        try
        {
            foreach (var evt in waveEvents_)
            {
                await evt.ExecuteAsync(this).AttachExternalCancellation(token);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Wave events canceled.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Wave event error: {ex}");
        }
    }

    private async UniTask SpawnEnemiesAsync(GameObject prefab, int count, CancellationToken token)
    {
        NativeArray<Vector3> positions = new(count, Allocator.TempJob);
        NativeArray<uint> randomSeeds = new(count, Allocator.TempJob);

        try
        {
            uint baseSeed = (uint)UnityEngine.Random.Range(1, int.MaxValue);
            for (int i = 0; i < count; i++)
            {
                randomSeeds[i] = baseSeed + (uint)(i * 997);
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
            await UniTask.WaitUntil(() => handle.IsCompleted, cancellationToken: token);
            handle.Complete();

            for (int i = 0; i < count; i++)
            {
                var enemy = Instantiate(prefab, positions[i], Quaternion.identity);
                enemy.GetComponent<EnemyBase>().Initialize(player_);
            }

            Debug.Log($"Spawned {count} of {prefab.name}");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"Spawn canceled for {prefab.name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Spawn error for {prefab.name}: {ex}");
        }
        finally
        {
            if (positions.IsCreated) positions.Dispose();
            if (randomSeeds.IsCreated) randomSeeds.Dispose();
        }
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