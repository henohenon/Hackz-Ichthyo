using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

abstract public class EnemyBase : MonoBehaviour
{
    [SerializeField] private IQ onDeathLearnAiIq_;
    [SerializeField] protected float hitPoint, speed, attackPower;
    [SerializeField] private Player.Player player_;

    protected Dictionary<ActionBase, float?> actionDurationPairs = new Dictionary<ActionBase, float?>();

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
                Debug.Log("pair.Key: " + pair.Key + "pair.Value: " + pair.Value);
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
            Destroy(gameObject);
        }
        if (Vector3.Distance(player_.transform.position, this.transform.position) < 0.2f)
        {
            OnDamage(attackPower);
            Destroy(gameObject);
        }
    }

    ///NOTE: 以下サンドボックスパタン
    protected Player.Player Player => player_;
}
