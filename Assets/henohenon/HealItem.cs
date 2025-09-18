using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField]
    private int healAmount = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Player.Player>(out var player))
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
