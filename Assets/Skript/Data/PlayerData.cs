using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public int startingMoney = 500, quotaMoney = 2000;
    private int money;

    public bool isGameOver;

    [SerializeField] private GameObject gameClearPanel, gameOverPanel;
    [SerializeField] private TextMeshProUGUI[] textHighScore;

    public int GetMoney => money;

    public void AddMoney(int _addMoney)
    {
        money += _addMoney;
        AudioManager.instance.PlaySFX(6);
    }

    public void SetMoney(int _setMoney)
    {
        money = _setMoney;
        AudioManager.instance.PlaySFX(3);
    }

    public static PlayerData instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        
        isGameOver = false; 
        SetMoney(startingMoney);
    }

    private void Start()
    {
    }

    public bool TakeMoney(int _cost)
    {
        if (_cost > money) return false;
        money -= _cost;
        
        return true;
    }
    public bool HaveEnoughMoney(int _cost)
    {
        if (_cost > money) return false;
        return true;
    }

    private void Update()
    {
        if (GetMoney >= quotaMoney) GameClear();
    }

    public void GameClear()
    {
        if(isGameOver) return;
        
        gameClearPanel.SetActive(true);
        textHighScore[1].text = $"เงินที่ได้รับ: {GetMoney} / {quotaMoney}฿";
        AudioManager.instance.PlaySFX(5);
        
        isGameOver = true;
    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        textHighScore[0].text = $"เงินที่ได้รับ: {GetMoney} / {quotaMoney}฿";
    }

    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(0);
    }
}
