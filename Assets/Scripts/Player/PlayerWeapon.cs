using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class PlayerWeapon : MonoBehaviour
{
    public event EventHandler OnShoot;
    public event EventHandler<OnWeaponChangeArgs> OnWeaponChange;
    public class OnWeaponChangeArgs : EventArgs
    {
        public Weapon weapon;
    }

    public GameObject startWeapon;

    public Transform playerWeaponSlot;
    [SerializeField]
    private GameObject currentWeapon;

    private bool canPickUpWeapon;
    private GameObject weaponToPickUp;
    [SerializeField]
    private Weapon weapon;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        currentWeapon = startWeapon;
        weapon = currentWeapon.GetComponent<Weapon>();
        weapon.SendMessage("OnPickUp", player.color);
    }

    private void Update()
    {
        if (weapon.ammo == 0)
        {
            OutOfAmmo();
        }
    }

    void Shoot()
    {
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    void PickUp()
    {
        if (canPickUpWeapon)
        {
            if (currentWeapon == startWeapon)
            {
                startWeapon.SendMessage("OnDrop");
                startWeapon.SetActive(false);
            }

            if (currentWeapon != null && currentWeapon != startWeapon)
                Drop(currentWeapon);

            weaponToPickUp.transform.SetParent(playerWeaponSlot);
            currentWeapon = weaponToPickUp;
            weapon = weaponToPickUp.GetComponent<Weapon>();

            weapon.SendMessage("OnPickUp", player.color);

            currentWeapon.transform.localPosition = new Vector3(0f, 0f, 0f);
            currentWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            OnWeaponChange?.Invoke(this, new OnWeaponChangeArgs { weapon = weapon });
        }
    }

    void Drop(GameObject _weapon)
    {
        _weapon.SendMessage("OnDrop");
        _weapon.transform.SetParent(null);
        currentWeapon = null;
        weapon = null;
    }

    void OutOfAmmo()
    {
        Drop(currentWeapon);

        canPickUpWeapon = false;
        weaponToPickUp = null;

        startWeapon.SetActive(true);
        currentWeapon = startWeapon;
        weapon = startWeapon.GetComponent<Weapon>();
        weapon.SendMessage("OnPickUp", player.color);

        OnWeaponChange?.Invoke(this, new OnWeaponChangeArgs { weapon = weapon });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            canPickUpWeapon = true;
            weaponToPickUp = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            canPickUpWeapon = false;
            weaponToPickUp = null;
        }
    }

    void OnDeath()
    {
        if (currentWeapon != startWeapon)
            Drop(currentWeapon);
    }

    void OnRespawn()
    {
        if (currentWeapon != startWeapon)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
            weapon = null;

            startWeapon.SetActive(true);
            currentWeapon = startWeapon;
            weapon = startWeapon.GetComponent<Weapon>();
            weapon.SendMessage("OnPickUp", player.color);

            OnWeaponChange?.Invoke(this, new OnWeaponChangeArgs { weapon = weapon });
        }
    }
}
