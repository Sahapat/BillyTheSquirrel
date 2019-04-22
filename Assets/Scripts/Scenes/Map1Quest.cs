using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Quest : MonoBehaviour
{
    [SerializeField]int indexQuestToSet = 0;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Map1>().SetCurrentQuest(indexQuestToSet);
            Destroy(this);
        }
    }
}
