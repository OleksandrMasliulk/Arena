using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Color color;

    private GameController gameController;
    private int playerIndex;
    private PlayerParameters playerParameters;
    private GameObject playerGraphics;
    private UIController uiController;

    public bool isAlive;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        playerParameters = GetComponent<PlayerParameters>();
        playerGraphics = transform.GetChild(0).gameObject;

        uiController = FindObjectOfType<UIController>();
    } 

    private void Start()
    {
        playerIndex = gameController.players.ToArray().Length;
        gameController.players.Add(this);

        uiController.PlayerJoin(playerIndex, color);

        DontDestroyOnLoad(this);
        transform.position = gameController.spawnPoints[playerIndex].position;

        gameController.OnRespawn += Respawn;
    }

    void OnDamageTaken(float damage)
    {
        Debug.Log(damage + " damage taken");
        playerParameters.health -= damage;

        if (playerParameters.health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isAlive = false;
        SendMessage("OnDeath");
        gameController.SendMessage("OnPlayerDie");
    }

    void Respawn(object sender, EventArgs e)
    {
        isAlive = true;
        SendMessage("OnRespawn");
        transform.position = gameController.spawnPoints[playerIndex].position + new Vector3(0, 2f, 0);
        uiController = FindObjectOfType<UIController>();
    }

}
