using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnviroumentShow : MonoBehaviour
{
    [SerializeField]GameObject[] showEnvirouments;
    [SerializeField]Text targetTxt = null;
    [SerializeField]string[] txtShows;
    [SerializeField]Vector3 LeftPos;
    [SerializeField]Vector3 CenterPos;
    [SerializeField]Vector3 RightPos;
    [SerializeField]float speed = 5f;
    int showIndex = 0;
    int previousShowIndex = -1;
    bool isSet = false;
    void Start()
    {
        foreach(GameObject temp in showEnvirouments)
        {
            temp.transform.position = RightPos;
        }
        showEnvirouments[0].transform.position = CenterPos;
    }
    void FixedUpdate()
    {
        if(!isSet)return;
        showEnvirouments[showIndex].transform.position = Vector3.MoveTowards(showEnvirouments[showIndex].transform.position,CenterPos,Time.deltaTime*speed);
        isSet = (showEnvirouments[showIndex].transform.position != CenterPos)?true:false;
        if(previousShowIndex == -1)return;
        showEnvirouments[previousShowIndex].transform.position = Vector3.MoveTowards(showEnvirouments[previousShowIndex].transform.position,LeftPos,Time.deltaTime*speed);
    }
    public void Next()
    {
        isSet = true;
        previousShowIndex = (showIndex == showEnvirouments.Length-1)?-1:showIndex;
        showIndex++;
        showIndex = Mathf.Clamp(showIndex,0,showEnvirouments.Length-1);
        targetTxt.text = txtShows[showIndex];
        showEnvirouments[showIndex].SetActive(true);
        if(previousShowIndex == -1)return;
        showEnvirouments[previousShowIndex].transform.position = CenterPos;
        showEnvirouments[showIndex].transform.position = RightPos;
    }
    public void Previous()
    {
        isSet = true;
        previousShowIndex = (showIndex == 0)?-1:showIndex;
        showIndex--;
        showIndex = Mathf.Clamp(showIndex,0,showEnvirouments.Length-1);
        targetTxt.text = txtShows[showIndex];
        showEnvirouments[showIndex].SetActive(true);
        if(previousShowIndex == -1)return;
        showEnvirouments[previousShowIndex].transform.position = CenterPos;
        showEnvirouments[showIndex].transform.position = LeftPos;
    }
    public void CloseAll()
    {
        foreach(GameObject temp in showEnvirouments)
        {
            temp.SetActive(false);
        }
    }
    public void OpenAll()
    {
        showEnvirouments[showIndex].SetActive(true);
        targetTxt.text = txtShows[showIndex];
    }
}
