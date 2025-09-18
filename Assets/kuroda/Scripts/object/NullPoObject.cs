using UnityEngine;

public class NullPoObject : MonoBehaviour
{
    public float speed = 1f; 
    private Rigidbody2D rb;
    private int attackPower = 2;
    private int lifetime = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Player player = other.GetComponent<Player.Player>();
            if (player != null)
            {
                player.OnDamage(attackPower); 
            }
            Destroy(gameObject);
        }
    }
}