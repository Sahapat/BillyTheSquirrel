using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Quest1 : MonoBehaviour
{
    [SerializeField]int indexQuestToSet = 0;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Map2>().SetCurrentQuest(indexQuestToSet);
            Destroy(this);
        }
    }
}
