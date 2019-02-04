using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public string CurrentLevelInfo
    {
        get
        {
            return $"Level {CurrentLevel}";
        }
    }
    public int CurrentLevel{get;private set;}=1;
}
