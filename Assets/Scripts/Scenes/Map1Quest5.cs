using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Quest5 : MonoBehaviour
{
    [SerializeField]int indexQuestToSet = 0;
    bool canShowQuest = false;
    Map1 map1 = null;
    void Start()
    {
        map1 = FindObjectOfType<Map1>();
    }
    void Update()
    {
        if(map1.currentQuest == 4)
        {
            canShowQuest = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(!canShowQuest)return;
        if(collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Map1>().SetCurrentQuest(indexQuestToSet);
            Destroy(this);
        }
    }
}
