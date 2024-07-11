using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;
    private IconHandler iconHandler;

    [SerializeField] private float secondsToWaitBeforeDeathCheck = 3f;
    
    [SerializeField] private int leftBird = 3;
    
    [SerializeField] private GameObject loseUIPanel;
    [SerializeField] private GameObject winUIPanel;
    private int usedBird = 0;

    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        if (gmInstance == null)
        {
            gmInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (loseUIPanel != null)
        {
            loseUIPanel.SetActive(false);
        }
        if (winUIPanel != null)
        {
            winUIPanel.SetActive(false);
        }

        iconHandler = FindObjectOfType<IconHandler>();

        Enemy[] enemys = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemys.Length; i++)
        {
            enemies.Add(enemys[i]);
        }
    }

    public void UseShot()
    {
        usedBird++;
        iconHandler.UseShot(usedBird);

        CheckWinLose();
    }

    public bool CheckBirdNumber()
    {
        if (usedBird < leftBird)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckWinLose()
    {
        if (usedBird == leftBird)
        {
            StartCoroutine(CheckAfterThisTime());
        }
    }

    private IEnumerator CheckAfterThisTime()
    {
        yield return new WaitForSeconds(secondsToWaitBeforeDeathCheck);
        if (enemies.Count == 0)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        CheckForAllDeadEnemy();
    }

    private void CheckForAllDeadEnemy()
    {
        if (enemies.Count == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (winUIPanel != null)
        {
            winUIPanel.SetActive(true);
        }
    }
    private void LoseGame()
    {
        if (loseUIPanel != null)
        {
            loseUIPanel.SetActive(true);
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
