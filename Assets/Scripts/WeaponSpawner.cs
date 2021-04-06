using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weaponPool;
    public float spawnRate;

    private void Start()
    {
        InvokeRepeating("SpawnWeapon", 0, spawnRate);
    }

    void SpawnWeapon()
    {
        GameObject weaponToSpawn = weaponPool[Random.Range(0, weaponPool.Length)];

        if (transform.childCount == 0)
            Instantiate(weaponToSpawn, transform);
    }
}
