using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab;

    public static Helper Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate Helper instance detected. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// 指定位置にダメージテキストを表示する
    /// </summary>
    /// <param name="position">ワールド座標</param>
    public void ShowDamage(Vector3 position, int damage)
    {
        if (damageTextPrefab == null)
        {
            Debug.LogWarning("DamageText prefab is not assigned.");
            return;
        }

        var damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        damageText.OnShowTextAsync(damage);
    }
}