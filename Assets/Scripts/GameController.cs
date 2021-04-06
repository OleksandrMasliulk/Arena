using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public event EventHandler OnRespawn;

    public List<Player> players;

    public Transform[] spawnPoints;

    private int scene = 1;
    private UIController uiController;
    public bool isPaused = false;

    private void Awake()
    {
        Clean();
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        Debug.Log("START");
        players = new List<Player>();

        FindSpawnPoints();

        SceneManager.sceneLoaded += OnSceneLoaded;
        uiController.OnMainMenu += MainMenu;
        DontDestroyOnLoad(this);
    }

    void Clean()
    {
        GameController[] controllers = FindObjectsOfType<GameController>();
        if (controllers.Length > 1)
            Destroy(this.gameObject);
    }

    void FindSpawnPoints()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = points[i].transform;
        }
    }

    void OnPlayerDie()
    {
        Player[] playersAlive = players.ToArray();

        int playerAlive = 0;
        int count = 0;
        for (int i = 0; i < playersAlive.Length; i++)
            if (playersAlive[i].isAlive)
            {
                count++;
                playerAlive = i;
            }

        if (count == 1)
        {
            uiController.PlayerWin(playerAlive, players[playerAlive].color);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2);

        if (scene == 3)
            scene = 1;
        else
            scene++;

        FindObjectOfType<LevelLoader>().LoadLevel(scene);
        OnRespawn?.Invoke(this, EventArgs.Empty);
    }

    void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        Time.timeScale = 1;
        DestroyWeapon();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            FindSpawnPoints();
            uiController = FindObjectOfType<UIController>();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            uiController.Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        uiController.Resume();
    }

    void DestroyPlayers()
    {
        foreach (Player player in players)
        {
            Debug.Log(player.name);
            Destroy(player.gameObject);
        }
        players = null;
    }

    void DestroyWeapon()
    {
        Weapon[] weapons = FindObjectsOfType<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            if (weapon.weaponType != Weapon.WeaponType.Pistol)
                Destroy(weapon.gameObject);
        }
    }

    void MainMenu(object sender, EventArgs e)
    {
        DestroyPlayers();
        Destroy(this.gameObject);
    }
}
