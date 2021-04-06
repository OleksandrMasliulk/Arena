using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.root.SendMessage("OnDamageTaken", 100);
        } 
        else if (other.tag == "Weapon")
        {
            Destroy(other.gameObject);
        }
    }
}
