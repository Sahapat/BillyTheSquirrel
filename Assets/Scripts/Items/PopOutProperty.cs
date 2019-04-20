using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PopOutProperty
{
    public GameObject objectToPopOut = null;
    [Range(0,1)]float PopOutChance = 0f;
    
    public bool isRandomNum = false;
    public int minRandom = 0;
    public int maxRandom = 0;
    
    [Range(0,10)] int PopOutNum = 0;

    public bool GetPopOutChance()
    {
        return (PopOutChance ==1)?true:(Random.value <= PopOutChance);
    }
    public int GetNumPopOut()
    {
        return (isRandomNum)?Random.Range(minRandom,maxRandom+1):PopOutNum;
    }
}