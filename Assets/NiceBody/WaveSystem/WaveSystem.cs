using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public sealed class WaveSystem : MonoBehaviour
{
    [SerializeField] private Player.Player player_;
    [SerializeField] private float spawnEnemyDistsanceRange_;
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
                SpawnEnemies(enemyData.GameObject_, enemyData.Count_);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(wave.CooldownSeconds));

            Debug.Log($"Wave End: {wave.name}");
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

    private void SpawnEnemies(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // スポーン範囲を定義（例：X: -5〜5, Y: -3〜3）
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-3f, 3f);
            Vector3 spawnPosition = new Vector3(x * spawnEnemyDistsanceRange_, y * spawnEnemyDistsanceRange_, 0f) + player_.transform.position;

            var enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyBase>().Initialize(player_);
        }

        Debug.Log($"Spawned {count} of {prefab.name}");
    }
}