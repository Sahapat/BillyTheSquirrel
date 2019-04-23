using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadHelper : MonoBehaviour
{
    public static int GetRandomLevel(string levelPreset)
    {
        int levelToRandom = -1;
        switch(levelPreset)
        {
            case "Level1":
                levelToRandom = Random.Range(1,1);
            break;
            case "Level2":
                levelToRandom = Random.Range(2,2);
            break;
            case "Finish":
                levelToRandom = 3;
            break;
        }
        return levelToRandom;
    }
}
