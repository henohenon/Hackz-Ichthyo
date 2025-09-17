using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

abstract public class EnemyBase : MonoBehaviour
{
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

    protected void onDamage(float damage)
    {
        this.hitPoint -= damage;
    }
    abstract protected void SetActions();

    void Start()
    {
        SetActions();
        OnAction();
    }

    void Update()
    {
        if (hitPoint <= 0)
        {
            Destroy(this.gameObject);
        }
        if (Vector3.Distance(player_.transform.position, this.transform.position) < 0.2f)
        {
            onDamage(attackPower);
            Destroy(this.gameObject);
        }
    }

    ///NOTE: 以下サンドボックスパタン
    protected Player.Player Player => player_;
}
