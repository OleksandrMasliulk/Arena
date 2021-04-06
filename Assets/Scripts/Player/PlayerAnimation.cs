using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerInputController inputController;

    public Transform playerWeaponSlot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        GetComponent<PlayerWeapon>().OnWeaponChange += OnWeaponSwitch;
    }

    private void Update()
    {
        Vector3 final = Quaternion.AngleAxis(Mathf.Atan2(inputController.rotationInput.x, inputController.rotationInput.y) * Mathf.Rad2Deg, Vector3.up) * inputController.movementInput;

        //float finalY = inputController.movementInput.x * inputController.rotationInput.x - inputController.rotationInput.y * inputController.movementInput.z;
        //float finalX = inputController.movementInput.x * inputController.rotationInput.y - inputController.rotationInput.x * inputController.movementInput.z;

        animator.SetFloat("Horizontal", final.x);
        animator.SetFloat("Vertical", final.z);
    }

    void DiasbleAllLayers()
    {
        for (int i = 1; i < animator.layerCount; i++)
            animator.SetLayerWeight(i, 0);
    }

    void OnWeaponSwitch(object sender, PlayerWeapon.OnWeaponChangeArgs e)
    {
        if (e.weapon.weaponType == WeaponType.Pistol)
        {
            DiasbleAllLayers();
            animator.SetLayerWeight(1, 1);
        }
        else if (e.weapon.weaponType == WeaponType.Rifle)
        {
            DiasbleAllLayers();
            animator.SetLayerWeight(2, 1);
        }
        else if (e.weapon.weaponType == WeaponType.Shotgun)
        {
            DiasbleAllLayers();
            animator.SetLayerWeight(3, 1);
        } else if (e.weapon.weaponType == WeaponType.SniperRifle)
        {
            DiasbleAllLayers();
            animator.SetLayerWeight(4, 1);
        }
        else 
        {
            DiasbleAllLayers();
            animator.SetLayerWeight(5, 1);
        }
    }
}
