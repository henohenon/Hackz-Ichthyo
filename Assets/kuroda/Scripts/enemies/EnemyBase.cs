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
    private float nextAttackTime = 0f;
    [SerializeField] protected Player.Player player_;

    protected Dictionary<ActionBase, float?> actionDurationPairs = new Dictionary<ActionBase, float?>();
    private bool isPlayerInRange = false;
    public void Initialize(Player.Player player)
    {
        player_ = player;
    }

    protected async Task OnAction()
    {
        Context context = SetContext();
        while (true)
        {
            foreach (KeyValuePair<ActionBase, float?> pair in actionDurationPairs)
            {
                await pair.Key.DoAction(context, pair.Value);
            }
        }
    }
    protected Context SetContext()
    {
        Context context = new Context();
        context.EnemyTransform = this.transform;
        context.PlayerTransform = Player.transform;
        context.PlayerInput = Player.GetInput();
        context.speed = this.speed;
        context.EnemySpriteRenderer = GetComponent<SpriteRenderer>();
        context.CoroutineRunner = this;
        context.EnemyRigidbody2D = GetComponent<Rigidbody2D>();
        return context;
    }

    public void OnDamage(float damage)
    {
        this.hitPoint -= damage;

        if (Helper.Instance != null)
        {
            Helper.Instance.ShowDamage(transform.position, (int)damage);
        }
        else
        {
            Debug.LogWarning("Helper.Instance is null. Damage text not shown.");
        }
    }
    protected bool IsDeath() => hitPoint <= 0;

    abstract protected void SetActions();

    void Start()
    {
        SetActions();
        OnAction();
    }

    void Update()
    {
        if (IsDeath())
        {
            Player.AddIQ(onDeathLearnAiIq_);
            var instance = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            instance.Initialize(onDeathLearnAiIq_.Value);
            Destroy(gameObject);
        }
        if (isPlayerInRange && Time.time >= nextAttackTime)
        {
            OnPlayerHit();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    protected void  OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 出ていったのがPlayerかどうかTagで判定
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    abstract protected void OnPlayerHit();

    ///NOTE: 以下サンドボックスパタン
    protected Player.Player Player => player_;
}
