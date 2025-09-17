using UnityEngine;

public class NullPoObject : MonoBehaviour
{
    public float speed = 2f; 
    private Rigidbody2D rb;
    private int attackPower = 2;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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