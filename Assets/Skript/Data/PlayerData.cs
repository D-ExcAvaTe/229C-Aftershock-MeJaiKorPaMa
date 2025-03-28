using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int startingMoney = 500, quotaMoney = 2000;
    private int money;

    public int GetMoney => money;
    public void AddMoney(int _addMoney) => money += _addMoney;
    public void SetMoney(int _setMoney) => money = _setMoney;

    public static PlayerData instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        SetMoney(startingMoney);
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
}
