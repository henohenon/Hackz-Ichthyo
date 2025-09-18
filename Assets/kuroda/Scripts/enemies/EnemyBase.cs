using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NiceBody.Player.LearnedSkill;
using UnityEngine;

abstract public class EnemyBase : MonoBehaviour
{
    [SerializeField] private IQ onDeathLearnAiIq_;
    [SerializeField] private DeathEffect deathEffectPrefab;
    [SerializeField] protected float hitPoint, speed, attackPower;
    protected float attackInterval = 0.5f;
    private float initHitpoint;
    private float nextAttackTime = 0f;
    [SerializeField] protected Player.Player player_;

    protected Dictionary<ActionBase, float?> actionDurationPairs = new Dictionary<ActionBase, float?>();
    private bool isPlayerInRange = false;
    public void Initialize(Player.Player player)
    {
        player_ = player;
        hitPoint = initHitpoint;
    }

    protected async Task OnAction()
    {
        if (!gameObject.activeSelf) return;
        Context context = SetContext();
        while (gameObject.activeSelf)
        {
            foreach (KeyValuePair<ActionBase, float?> pair in actionDurationPairs)
            {
                await pair.Key.DoAction(context, pair.Value);
                if (!gameObject.activeSelf) return;
            }
        }
    }
    protected Context SetContext()
    {
        Context context = new()
        {
            EnemyTransform = this.transform,
            PlayerTransform = Player.transform,
            PlayerInput = Player.GetInput(),
            speed = this.speed,
            EnemySpriteRenderer = GetComponent<SpriteRenderer>(),
            CoroutineRunner = this,
            EnemyRigidbody2D = GetComponent<Rigidbody2D>()
        };
        return context;
    }

    public void OnDamage(float damage)
    {
        if (!gameObject.activeSelf) 
            return;

        hitPoint -= damage + player_.AttackPower_;

        if (Helper.Instance != null)
        {
            Helper.Instance.ShowDamage(transform.position, (int)damage+player_.AttackPower_);
        }
        else
        {
            Debug.LogWarning("Helper.Instance is null. Damage text not shown.");
        }
    }
    protected bool IsDeath() => hitPoint <= 0;

    abstract protected void SetActions();

    private void Awake()
    {
        initHitpoint = hitPoint;
        SetActions();
    }

    void OnEnable()
    {
        OnAction();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;
        if (IsDeath())
        {
            Player.AddIQ(onDeathLearnAiIq_);
            var instance = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            instance.Initialize(onDeathLearnAiIq_.Value);
            gameObject.SetActive(false);
            player_ = null;
            return;
            // Destroy(gameObject);
        }
        if (isPlayerInRange)
        {
            OnPlayerHit();
        }
    }

    protected void  OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;
        // 出ていったのがPlayerかどうかTagで判定
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OnDestroy()
    {
        foreach (var (key, value) in actionDurationPairs)
        {
            key.Dispose();
        }
        actionDurationPairs.Clear();
    }


    abstract protected void OnPlayerHit();

    ///NOTE: 以下サンドボックスパタン
    protected Player.Player Player => player_;
}
