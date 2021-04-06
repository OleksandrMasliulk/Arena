using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;

    public float transitionTime;

    public void LoadLevel(int index)
    {
        StartCoroutine(Load(index));
    }

    IEnumerator Load(int index)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(index);
    }
}
