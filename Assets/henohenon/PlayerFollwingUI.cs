using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollwingUI : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        transform.position = player.position;
    }
}
