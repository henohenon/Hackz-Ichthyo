using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Game/Event/SpawnBoss")]
public sealed class SpawnBoss : EventBase
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    public override async UniTask ExecuteAsync(WaveSystem waveSystem)
    {
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Boss spawned: {bossPrefab.name}");
    }
}