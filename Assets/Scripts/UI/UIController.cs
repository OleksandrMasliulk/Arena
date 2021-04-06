using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public event EventHandler OnMainMenu;

    public GameObject pausePanel;
    public TextMeshProUGUI playerText;

    public void PlayerJoin(int index, Color color)
    {
        TextMeshProUGUI text = Instantiate(playerText, transform);

        text.color = color;
        text.text = "PLAYER" + (index + 1) + " HAS JOINED";
    }

    public void PlayerWin(int index, Color color)
    {
        TextMeshProUGUI text = Instantiate(playerText, transform);

        text.color = color;
        text.text = "PLAYER" + (index + 1) + " WINS";
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        pausePanel.GetComponent<Animator>().Play("PausePanel_In");
    }

    public void Resume()
    {
        pausePanel.GetComponent<Animator>().Play("PausePanel_Out");
    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke(this, EventArgs.Empty);
        FindObjectOfType<LevelLoader>().LoadLevel(0);
    }
}
