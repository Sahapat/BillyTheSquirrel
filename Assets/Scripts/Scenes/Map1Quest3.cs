using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Quest3 : MonoBehaviour
{
    [SerializeField]int indexQuest = 0;
    void Update()
    {
        if(GameCore.m_GameContrller.ClientPlayerTarget.killEnemyCount > 0)
        {
            FindObjectOfType<Map1>().SetCurrentQuest(indexQuest);
            Destroy(this);
        }
    }
}
