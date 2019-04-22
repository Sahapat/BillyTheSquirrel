using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Temp
{
    public static int maxHP = 100;
    public static int maxSP = 100;

    public static int Coin = 0;
}
public class Map2 : MonoBehaviour
{
    bool isSet = false;
    void Update()
    {
        if (!isSet)
        {
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterHP.SetMaxHP(Temp.maxHP);
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterStemina.SetMaxSP(Temp.maxSP);
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin = Temp.Coin;

            GameCore.m_uiHandler.UpdateHPMax();
            GameCore.m_uiHandler.UpdateSPMax();
            GameCore.m_GameContrller.GameStart();
            isSet = true;
        }
    }
}
