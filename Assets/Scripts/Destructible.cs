using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float fadeTime;

    private float time;
    private float startTime;
    private bool isFading;

    private MeshRenderer mesh;
    private Color startColor;
    private Color transparent;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        fadeTime += Random.Range(-1f, 1f);

        startColor = mesh.material.color;
        transparent = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Debug.Log(startColor + " // " + transparent);
    }

    private void Update()
    {
        if (isFading)
        {
            time = Time.time - startTime;
            mesh.material.color = Color.Lerp(startColor, transparent, time / fadeTime);

            if (fadeTime - time <= 0)
                Destroy(this.gameObject);
        }
    }

    public void FadeOut()
    {
        mesh.enabled = false;
        mesh.material.SetFloat("_Surface", 1f);
        mesh.enabled = true;

        isFading = true;
        startTime = Time.time;
    }
}
