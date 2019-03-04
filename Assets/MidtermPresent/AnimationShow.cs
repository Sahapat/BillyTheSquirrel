using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationShow : MonoBehaviour
{
    [SerializeField]Text targetTxt = null;
    [SerializeField]string[] showTxts;
    [SerializeField]GameObject[] showModels;
    [SerializeField]Animator[] showAnim;

    int showIndex = 0;
    void Start()
    {
        showIndex = Mathf.Clamp(showIndex,0,showTxts.Length-1);
        foreach(GameObject temp in showModels)
        {
            temp.SetActive(false);
        }
        showModels[showIndex].SetActive(true);
        targetTxt.text = showTxts[showIndex];
    }
    public void PlayTrigger(string name)
    {
        showAnim[showIndex].SetTrigger(name);
    }
    public void Next()
    {
        showIndex++;
        showIndex = Mathf.Clamp(showIndex,0,showTxts.Length-1);
        foreach(GameObject temp in showModels)
        {
            temp.SetActive(false);
        }
        showModels[showIndex].SetActive(true);
        targetTxt.text = showTxts[showIndex];
    }
    public void Previos()
    {
        showIndex--;
        showIndex = Mathf.Clamp(showIndex,0,showTxts.Length-1);
        foreach(GameObject temp in showModels)
        {
            temp.SetActive(false);
        }
        showModels[showIndex].SetActive(true);
        targetTxt.text = showTxts[showIndex];
    }
    public void CloseAll()
    {
        foreach(GameObject temp in showModels)
        {
            temp.SetActive(false);
        }
    }
    public void OpenAll()
    {
        showIndex = Mathf.Clamp(showIndex,0,showTxts.Length-1);
        foreach(GameObject temp in showModels)
        {
            temp.SetActive(false);
        }
        showModels[showIndex].SetActive(true);
        targetTxt.text = showTxts[showIndex];
    }

}
