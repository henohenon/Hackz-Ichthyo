using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Context : MonoBehaviour
{
    public Transform EnemyTransform { get; set; }
    public Transform PlayerTransform { get; set; }
    public float speed { get; set; }
    public Player.Input PlayerInput { get; set; }
    public SpriteRenderer EnemySpriteRenderer { get; set; }
    public MonoBehaviour CoroutineRunner { get; set; }

}
