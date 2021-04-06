using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
