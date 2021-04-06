using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Rifle,
        Shotgun,
        SniperRifle,
        RocketLauncher
    }

    public WeaponType weaponType;

    //All weapons
    [Header("All weapon types")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public int ammo;

    public float timeBtwShots;
    public bool canShoot;

    private Color playerColor;
    public GameObject shootEffectPrefab;

    public Rigidbody rb;
    public BoxCollider triggerBox;
    public BoxCollider collision;

    //Rifle
    [Header("Rifle")]
    public int bulletsInARow;
    public float timeBtwBullets;

    //Shotgun
    [Header("Shotgun")]
    public int palletCount;
    public float spread;

    //Rocket Launcher
    [Header("Rocket Launcher")]
    public GameObject exhaustEffectPrefab;
    public Transform exhaustPosition;

    private AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (weaponType == WeaponType.Pistol)
            audioManager.InitSound(this.gameObject, "Pistol");
        else if (weaponType == WeaponType.Rifle)
            audioManager.InitSound(this.gameObject, "Pistol");
        else if (weaponType == WeaponType.Shotgun)
            audioManager.InitSound(this.gameObject, "Shotgun");
        else if (weaponType == WeaponType.SniperRifle)
            audioManager.InitSound(this.gameObject, "SniperRifle");
        else if (weaponType == WeaponType.RocketLauncher)
            audioManager.InitSound(this.gameObject, "RocketLauncher");
    }

    void Shoot(object sender, EventArgs e)
    {
        if (canShoot && ammo > 0)
        {
            if (weaponType == WeaponType.Pistol)
            {
                audioManager.Play(this.gameObject);
                GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                SpawnEffects();
                bullet.GetComponent<Projectile>().Setup(projectileSpawnPoint.up, transform.right, playerColor);
            }
            else if (weaponType == WeaponType.Rifle)
            {
                StartCoroutine(RapidFire());
            }
            else if (weaponType == WeaponType.Shotgun)
            {
                audioManager.Play(this.gameObject);
                SpawnEffects();
                for (int i = 0; i < palletCount; i++)
                {
                    GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                    Vector3 direction = projectileSpawnPoint.up + projectileSpawnPoint.right * (UnityEngine.Random.Range(-spread, spread)); //+ projectileSpawnPoint.forward * UnityEngine.Random.Range(-spread, spread);
                    bullet.GetComponent<Projectile>().Setup(direction, transform.right, playerColor);
                }

                ammo--;
            }
            else if (weaponType == WeaponType.SniperRifle)
            {
                audioManager.Play(this.gameObject);
                GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                SpawnEffects();
                bullet.GetComponent<Projectile>().Setup(projectileSpawnPoint.up, transform.right, playerColor);

                ammo--;
            }
            else
            {
                audioManager.Play(this.gameObject);
                GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                SpawnEffects();
                bullet.GetComponent<Projectile>().Setup(projectileSpawnPoint.up, transform.right, playerColor);

                ammo--;
            }

            StartCoroutine(WaitForNextShot());
        }
    }

    IEnumerator RapidFire()
    {
        for (int i = 0; i < bulletsInARow; i++)
        {
            audioManager.Play(this.gameObject);
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            SpawnEffects();
            bullet.GetComponent<Projectile>().Setup(projectileSpawnPoint.up, transform.right, playerColor);
            yield return new WaitForSeconds(timeBtwBullets);
        }

        ammo--;
    }

    IEnumerator WaitForNextShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBtwShots);
        canShoot = true;
    }

    void OnPickUp(Color _color)
    {
        transform.root.GetComponent<PlayerWeapon>().OnShoot += Shoot;
        playerColor = _color;

        if (weaponType != WeaponType.Pistol)
        {
            triggerBox.enabled = false;
            collision.enabled = false;
            rb.isKinematic = true;
        }
    }

    void OnDrop()
    {
        transform.root.GetComponent<PlayerWeapon>().OnShoot -= Shoot;

        if (weaponType != WeaponType.Pistol)
        {
            if (ammo > 0)
            {
                triggerBox.enabled = true;
                collision.enabled = true;
                rb.isKinematic = false;
            }
            else
            {
                collision.enabled = true;
                rb.isKinematic = false;
            }
        }
    }
    
    void SpawnEffects()
    {
        if (weaponType == WeaponType.RocketLauncher)
        {
            GameObject exhaustEffect = Instantiate(exhaustEffectPrefab, exhaustPosition.position, Quaternion.identity);
            exhaustEffect.transform.SetParent(exhaustPosition);;
            exhaustEffect.transform.localScale = new Vector3(1, 1, 1);
            ParticleSystemRenderer particles = exhaustEffect.GetComponent<ParticleSystemRenderer>();

            Vector3 direction = (exhaustPosition.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            exhaustEffect.transform.rotation = Quaternion.Euler(0, angle, 0);

            particles.material.color = playerColor * 2;
            particles.material.SetColor("_EmissionColor", playerColor * 2);
            particles.trailMaterial.color = playerColor * 2;
            particles.trailMaterial.SetColor("_EmissionColor", playerColor * 2);
        }
        else
        {
            GameObject shootEffect = Instantiate(shootEffectPrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
            shootEffect.transform.SetParent(projectileSpawnPoint);
            shootEffect.transform.localScale = new Vector3(1, 1, 1);
            ParticleSystemRenderer particles = shootEffect.GetComponent<ParticleSystemRenderer>();

            Vector3 direction = (projectileSpawnPoint.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            shootEffect.transform.rotation = Quaternion.Euler(0, angle, 0);

            particles.material.color = playerColor * 2;
            particles.material.SetColor("_EmissionColor", playerColor * 2);
            particles.trailMaterial.color = playerColor * 2;
            particles.trailMaterial.SetColor("_EmissionColor", playerColor * 2);
        }
    }
}
