using System;
using ButchersGames;
using Player;
using TMPro;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumber;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject failScreen;
    [SerializeField] private GameObject tutorial;
    
    [NonSerialized] public static GameManager Instance;

    private PlayerMoneyComponent playerMoney;

    [NonSerialized] public bool isGameStart;
    [NonSerialized] public bool waitingForSwipe = true;

    private void Start()
    {
        Instance = this;
        LevelManager.Default.SelectLevel(0);
        playerMoney = FindObjectOfType<PlayerMoneyComponent>();
    }

    public void StartGame()
    {
        isGameStart = true;
        waitingForSwipe = false;
        tutorial.SetActive(false);
    }

    public void Win()
    {
        winScreen.SetActive(true);
        isGameStart = false;
        PlayerAudio.Instance.Win();
    }

    public void Fail()
    {
        failScreen.SetActive(true);
        isGameStart = false;
        PlayerAudio.Instance.Fail();
    }

    private void Continue(bool success = false)
    {
        playerMoney.ResetMoney(success);
        isGameStart = true;
    }

    public void Retry()
    {
        PlayerAudio.Instance.Click();
        failScreen.SetActive(false);
        LevelManager.Default.RestartLevel();
        Continue();
    }

    public void NextLevel()
    {
        PlayerAudio.Instance.Click();
        winScreen.SetActive(false);
        LevelManager.Default.NextLevel();
        levelNumber.text = LevelManager.CurrentLevel.ToString();
        Continue(true);
    }
}