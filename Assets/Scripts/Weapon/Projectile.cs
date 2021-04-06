using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ProjectileType
    {
        Bullet,
        Pallet,
        Rocket
    }

    public ProjectileType projectileType;
    public Rigidbody rb;

    public float damage;
    public float speed;
    public float explosionRadius;
    public float impulse;
    public float maxDistance;
    public GameObject explosionPrefab;

    private Vector3 startPosition;
    private Color explosionColor;

    private void Start()
    {
        maxDistance *= Random.Range(.9f, 1.1f);
        startPosition = transform.position;

        if (projectileType == ProjectileType.Rocket)
        {
            FindObjectOfType<AudioManager>().InitSound(this.gameObject, "Rocket");
            FindObjectOfType<AudioManager>().Play(this.gameObject);
        }
    }

    private void Update()
    {
        float distance = (transform.position - startPosition).magnitude;
        if (distance > maxDistance)
        {
            if (transform.childCount > 0)
                transform.GetChild(0).SetParent(null);
            Destroy(this.gameObject);
        }
    }

    public void Setup(Vector3 direction, Vector3 rotation, Color color)
    {
        transform.rotation = Quaternion.AngleAxis(-90, rotation);

        rb.AddForce(direction * speed, ForceMode.Impulse);

        if (projectileType == ProjectileType.Bullet || projectileType == ProjectileType.Pallet)
        {
            GetComponent<MeshRenderer>().material.color = color * 2;
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color * 2);
            transform.GetChild(0).GetComponent<TrailRenderer>().material.color = color * 2;
            transform.GetChild(0).GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", color * 2);
        }
        else
        {
            transform.GetChild(0).GetComponent<TrailRenderer>().material.color = color * 2;
            transform.GetChild(0).GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", color * 2);
            transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material.color = color * 2;
            transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", color * 2);

            explosionColor = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Projectile" && other.tag != "Weapon")
        {
            Debug.Log("HIT");
            if (projectileType == ProjectileType.Rocket)
                Explode();
            else if (projectileType == ProjectileType.Bullet || projectileType == ProjectileType.Pallet)
                Hit(other);

            if (transform.childCount > 0)
                transform.GetChild(0).SetParent(null);
            Destroy(this.gameObject);
        }
    }

    void Hit(Collider collider)
    {
        Vector3 direction = transform.position - startPosition;
        if (collider.tag == "Player")
        {
            collider.transform.parent.SendMessage("OnDamageTaken", damage);
            collider.transform.parent.GetComponent<Rigidbody>().AddForce((collider.transform.position + direction).normalized * impulse, ForceMode.Impulse);
        }
    }

    void Explode()
    {
        
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().InitSound(explosion, "Explosion");
        FindObjectOfType<AudioManager>().Play(explosion);
        ParticleSystemRenderer explosionParticles = explosion.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
        explosionParticles.material.color = explosionColor * 2;
        //explosionParticles.material.SetColor("_EmissionColor", explosionColor * 2);

        foreach (Collider collider in affectedObjects)
        {
            Vector3 direction = collider.transform.position - transform.position;
            if (collider.tag == "Player")
            {
                collider.transform.parent.SendMessage("OnDamageTaken", damage / (direction.magnitude + 1));
                collider.transform.parent.GetComponent<Rigidbody>().AddForce((collider.transform.position + direction).normalized * impulse / (direction.magnitude + 1), ForceMode.Impulse);
            } 
            else if (collider.tag == "DestructibleObject")
            {
                collider.GetComponent<Rigidbody>().isKinematic = false;
                collider.GetComponent<Rigidbody>().AddForce((collider.transform.position + direction).normalized * impulse / (direction.magnitude + 1), ForceMode.Impulse);
                collider.GetComponent<Destructible>().FadeOut();
            }
        }
    }
}
