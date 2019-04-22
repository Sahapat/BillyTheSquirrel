using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Finish : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameCore.m_Main.LoadGameScene("Level2");
            Temp.maxHP = GameCore.m_GameContrller.ClientPlayerTarget.CharacterHP.MaxHP;
            Temp.maxSP = GameCore.m_GameContrller.ClientPlayerTarget.CharacterStemina.MaxSP;
            Destroy(this);
        }
    }
}
