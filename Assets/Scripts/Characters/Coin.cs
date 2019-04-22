using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin
{
    public delegate void _FuncInt(int value);
    public event _FuncInt OnCoinAdd;
    public event _FuncInt OnCoinRemove;

    public int _Coin;

    public void AddCoin(int value)
    {
        _Coin+=value;
        OnCoinAdd?.Invoke(value);
    }
    public void RemoveCoin(int value)
    {
        _Coin-=value;
        if(_Coin < 0)_Coin = 0;
        OnCoinRemove?.Invoke(value);
    }
}
