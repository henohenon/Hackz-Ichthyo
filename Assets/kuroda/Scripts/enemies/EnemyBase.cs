using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

abstract public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float hitPoint, speed;
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
        context.speed = this.speed;
        return context;
    }

    public void OnDamage(float damage)
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
    }

    ///NOTE: 以下サンドボックスパタン
    protected Player.Player Player => player_;
}
