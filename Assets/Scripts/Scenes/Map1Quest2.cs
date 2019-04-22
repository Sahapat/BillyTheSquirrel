using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Quest2 : MonoBehaviour
{
    [SerializeField] int indexQuestToSet = 0;
    void Update()
    {
        if (
        GameCore.m_GameContrller.ClientPlayerTarget.WeaponInventory)
        {
            FindObjectOfType<Map1>().SetCurrentQuest(indexQuestToSet);
            Destroy(this);
        }
    }
}
