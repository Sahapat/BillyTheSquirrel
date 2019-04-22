using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Quest4 : MonoBehaviour
{
    [SerializeField]int indexQuest = 0;
    Map1 map1 = null;
    void Awake()
    {
        map1=FindObjectOfType<Map1>();
    }
    void Update()
    {
        if(map1.currentQuest == 3)
        {
            if(GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin >= 50)
            {
                map1.SetCurrentQuest(indexQuest);
                Destroy(this);
            }
        }
    }
}
