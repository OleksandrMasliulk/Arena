using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    private SkinnedMeshRenderer[] meshes;
    private Color color;

    private Animator animator;
    private PlayerAnimation playerAnimation;
    private Player player;

    public Collider playerCollider;
    public Transform metarig;
    public GameObject bloodParticlesPrefab;
    public Vector3 offset;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();

        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        SetColor();
        player.color = color;
        TurnOffRagdoll();
    }

    private void Start()
    {
    }

    void SetColor()
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        foreach (SkinnedMeshRenderer mesh in meshes)
            if (mesh.tag != "Weapon")
                mesh.material.color = color;
    }

    void OnDamageTaken()
    {
        GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position + offset, Quaternion.identity);
        ParticleSystemRenderer particles = bloodParticles.GetComponent<ParticleSystemRenderer>();
        particles.material.color = color;
        particles.trailMaterial.color = color;
        //explosionParticles.material.SetColor("_EmissionColor", explosionColor * 2);

        StartCoroutine(Hurt());
    }

    IEnumerator Hurt()
    {
        foreach (SkinnedMeshRenderer mesh in meshes)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(1f);

        foreach (SkinnedMeshRenderer mesh in meshes)
            mesh.material.color = color;
    }

    void TurnOffRagdoll()
    {
        Rigidbody[] bones = metarig.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bones)
            rb.isKinematic = true;

        Collider[] colliders = metarig.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = false;

        playerCollider.enabled = true;
        animator.enabled = true;
        playerAnimation.enabled = true;
    }

    void TurnOnRagdoll()
    {
        Rigidbody[] bones = metarig.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bones)
            rb.isKinematic = false;

        Collider[] colliders = metarig.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = true;

        playerCollider.enabled = false;
        animator.enabled = false;
        playerAnimation.enabled = false;
    }

    void OnDeath()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        //TurnOnRagdoll();
    }

    void OnRespawn()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        //TurnOffRagdoll();
    }
}
