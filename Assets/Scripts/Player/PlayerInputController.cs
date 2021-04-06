using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 movementInput;
    public Vector3 rotationInput;

    void OnMoveVertical (InputValue value)
    {
        movementInput.z = value.Get<float>();
    }

    void OnMoveHorizontal(InputValue value)
    {
        movementInput.x = value.Get<float>();
    }

    void OnMove(InputValue value)
    {
        movementInput.x = value.Get<Vector2>().x;
        movementInput.z = value.Get<Vector2>().y;
    }

    void OnShoot()
    {
        this.SendMessage("Shoot");
    }

    void OnRotation_Gamepad(InputValue value)
    {
        if (value.Get<Vector2>().magnitude > 0)
            rotationInput = value.Get<Vector2>();
    }

    void OnRotation_Mouse(InputValue value)
    {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        rotationInput = value.Get<Vector2>() - (Vector2)playerPos;
        rotationInput.z = transform.position.y;
        rotationInput.Normalize();
    }

    void OnPickUpWeapon()
    {
        this.SendMessage("PickUp");
    }

    void OnPause()
    {
        FindObjectOfType<GameController>().Pause();
    }
}
