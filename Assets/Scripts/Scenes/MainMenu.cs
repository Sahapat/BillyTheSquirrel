using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Vector3[] cameraPagesPos = null;
    [SerializeField] Vector3[] cameraPageRotate = null;
    [SerializeField] GameObject[] Pages = null;
    [SerializeField] GameObject[] Controllers = null;

    int showIndex = 0;

    int pageIndex = 0;

    bool isSetPos = false;
    bool isSetRotate = false;

    void FixedUpdate()
    {
        if (!isSetPos)
        {
            if (transform.position != cameraPagesPos[pageIndex])
            {
                iTween.MoveTo(this.gameObject, cameraPagesPos[pageIndex], 2f);
                isSetPos = true;
            }
        }
        else
        {
            if (transform.position == cameraPagesPos[pageIndex])
            {
                isSetPos = false;
            }
        }
        if (!isSetRotate)
        {
            if (transform.rotation != Quaternion.Euler(cameraPageRotate[pageIndex]))
            {
                iTween.RotateTo(this.gameObject, cameraPageRotate[pageIndex], 2f);
                isSetRotate = true;
            }
        }
        else
        {
            if (transform.rotation == Quaternion.Euler(cameraPageRotate[pageIndex]))
            {
                isSetRotate = false;
            }
        }

    }
    public void ShowPageByIndex(int index)
    {
        foreach (GameObject temp in Pages)
        {
            temp.SetActive(false);
        }
        pageIndex = index;
        isSetPos = false;
        isSetRotate = false;
        Invoke("UpdateShowIndex",1.5f);
    }
    public void Next()
    {
        showIndex++;
        showIndex = Mathf.Clamp(showIndex, 0, Controllers.Length);
        foreach (GameObject temp in Controllers)
        {
            temp.SetActive(false);
        }
        Controllers[showIndex].SetActive(true);
    }
    public void Previous()
    {
        showIndex--;
        showIndex = Mathf.Clamp(showIndex, 0, Controllers.Length);
        foreach (GameObject temp in Controllers)
        {
            temp.SetActive(false);
        }

        isSetPos = false;
        isSetRotate = false;
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
    void UpdateShowIndex()
    {
        Pages[pageIndex].SetActive(true);
    }
}
