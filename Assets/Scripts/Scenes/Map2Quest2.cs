using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Quest2 : MonoBehaviour
{
    public GameObject[] chiefObject = null;

    bool isAllDead = false;
    void Update()
    {
        isAllDead = !chiefObject[0]&&chiefObject[1]&&!chiefObject[2]&&!chiefObject[3]&&!chiefObject[4];
        
        if(isAllDead)
        {
            GameCore.m_Main.LoadGameScene("Finish");
        }
    }   
}
