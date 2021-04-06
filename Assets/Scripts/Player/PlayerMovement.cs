using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputController inputController;
    private PlayerParameters playerParameters;
    private Player player;
    private GameController gameController;
    private AudioManager audioManager;

    private bool isWalking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<PlayerInputController>();
        playerParameters = GetComponent<PlayerParameters>();
        player = GetComponent<Player>();
        gameController = FindObjectOfType<GameController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.InitSound(this.gameObject, "Walk");
    }

    private void Update()
    {
        if (player.isAlive && !gameController.isPaused)
        {
            if (inputController.movementInput.magnitude > 0 && !isWalking)
            {
                audioManager.Play(this.gameObject);
                isWalking = true;
            }
            else if (inputController.movementInput.magnitude == 0 && isWalking)
            {
                audioManager.StopPlaying(this.gameObject);
                isWalking = false;
            }

            this.rb.MovePosition(rb.position + inputController.movementInput * playerParameters.movementSpeed * Time.deltaTime);

            float angle = Mathf.Atan2(inputController.rotationInput.x, inputController.rotationInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180f + angle, 0));
        }
    }
}
