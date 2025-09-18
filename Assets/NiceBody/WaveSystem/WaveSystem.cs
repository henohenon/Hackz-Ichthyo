using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameUnity;
using Player.State;
using R3;
using Random = UnityEngine.Random;

public sealed class WaveSystem : MonoBehaviour
{
    [SerializeField] private Player.Player player_;
    [SerializeField] private float spawnEnemyDistsanceRange_ = 1f;
    [SerializeField] private List<WaveData> waves_;
    [SerializeField] private List<EventBase> waveEvents_;
    [SerializeField] StartGame startGame_;
    [SerializeField] private SerializedDictionary<EnemyType, EnemyBase[]> enemyPrefabs = new();

    private Dictionary<EnemyType, List<EnemyBase>> enemyPool =
        new Dictionary<EnemyType, List<EnemyBase>>
    {

        { EnemyType.Fighter, new() },
        { EnemyType.Shooter, new() },
        { EnemyType.Boss, new() }
    
    };

    private readonly CancellationTokenSource cts_ = new();

    private async void Start()
    {
        await startGame_.StartGameAsync();


        player_.GetState<DeathState>()
               .OnDeath
               .Subscribe(_ =>
               {
                   Sound.PlaySE(SoundEffectType.GameOver);
                   Debug.LogWarning("Player died. Cancelling wave system.");
                   cts_.Cancel();
               })
               .AddTo(this);

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
            int waveIndex = 0;

            while (waveIndex < waves_.Count)
            {
                var wave = waves_[waveIndex];

                Debug.Log($"Starting wave {wave.name}. Required IQ: {wave.RequiredIQ.Value}, Current IQ: {player_.IQ.CurrentValue}");

                // 敵召喚は必ず実行
                foreach (var enemyData in wave.SummonEnemies)
                {
                    var activeEnemy = enemyPool[enemyData.Type].Where(e => !e.gameObject.activeSelf).ToList();
                    var activeDiff = enemyData.Count - activeEnemy.Count;
                    for (int i = 0; i < activeDiff; i++)
                    {
                        var prefabsArray = enemyPrefabs[enemyData.Type];
                        var prefab = prefabsArray[Random.Range(0, prefabsArray.Length)];
                        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        instance.gameObject.SetActive(false);

                        enemyPool[enemyData.Type].Add(instance);
                        activeEnemy.Add(instance);
                    }
                    await SpawnEnemiesAsync(activeEnemy, enemyData.Count, token);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(wave.CooldownSeconds), cancellationToken: token);

                // 次のウェーブに進む条件を判定
                if (waveIndex + 1 < waves_.Count &&
                    player_.IQ.CurrentValue < waves_[waveIndex + 1].RequiredIQ)
                {
                    Sound.PlaySE(SoundEffectType.WaveClear);
                    Debug.Log($"Waiting for IQ to reach {waves_[waveIndex + 1].RequiredIQ.Value} to start next wave.");
                    continue;
                }

                waveIndex++;
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

    private async UniTask SpawnEnemiesAsync(List<EnemyBase> objcts, int count, CancellationToken token)
    {
        NativeArray<Vector3> positions = new(count, Allocator.TempJob);
        NativeArray<uint> randomSeeds = new(count, Allocator.TempJob);

        try
        {
            uint baseSeed = (uint)Random.Range(1, int.MaxValue);
            
            var job = new EnemySpawnPositionJob
            {
                spawnRange = spawnEnemyDistsanceRange_,
                xRange = new Vector2(-5f, 5f),
                yRange = new Vector2(-3f, 3f),
                playerPosition = player_.transform.position,
                spawnPositions = positions,
                seed = baseSeed
            };

            JobHandle handle = job.Schedule(count, 32);
            await UniTask.WaitUntil(() => handle.IsCompleted, cancellationToken: token);
            handle.Complete();

            for (int i = 0; i < count; i++)
            {
                var instance = objcts[i];
                instance.transform.position = positions[i];
                instance.Initialize(player_);
                instance.gameObject.SetActive(true);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"Spawn canceled for"); //{obj.name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Spawn error for {ex}"); // {obj.name}: {ex}");
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
        [ReadOnly] public uint seed;

        public void Execute(int index)
        {
            var rand = new Unity.Mathematics.Random(seed + (uint)index * 997);

            var x = rand.NextFloat(xRange.x, xRange.y);
            var y = rand.NextFloat(yRange.x, yRange.y);
            
            var baseAngle =  rand.NextFloat(0f, 2f * Mathf.PI);
            var offsetX = Mathf.Cos(baseAngle) * spawnRange + x;
            var offsetY = Mathf.Sin(baseAngle) * spawnRange + y;
            var offset = new Vector3(offsetX, offsetY, 0);
            spawnPositions[index] = playerPosition + offset;
        }
    }
    
    public enum EnemyType
    {
        Fighter,
        Shooter,
        Boss
    }
}