using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject[] Pages = null;
    [SerializeField]GameObject[] Controllers = null;
    int showIndex =0;

    public void ShowPageByIndex(int index)
    {
        foreach(GameObject temp in Pages)
        {
            temp.SetActive(false);
        }
        Pages[index].SetActive(true);
    }
    public void Next()
    {
        showIndex++;
        showIndex = Mathf.Clamp(showIndex,0,Controllers.Length);
        foreach(GameObject temp in Controllers)
        {
            temp.SetActive(false);
        }
        Controllers[showIndex].SetActive(true);
    }
    public void Previous()
    {
        showIndex--;
        showIndex = Mathf.Clamp(showIndex,0,Controllers.Length);
        foreach(GameObject temp in Controllers)
        {
            temp.SetActive(false);
        }
        Controllers[showIndex].SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void LoadGameScene(string Levelpreset)
    {
        FindObjectOfType<FadeController>().LoadSceneAndFade(LevelLoadHelper.GetRandomLevel(Levelpreset));
    }
}
