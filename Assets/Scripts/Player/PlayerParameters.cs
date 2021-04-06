using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public float movementSpeed;
    public float health;

    void OnRespawn()
    {
        health = 100f;
    }
}
