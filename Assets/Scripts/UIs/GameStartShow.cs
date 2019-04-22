using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartShow : MonoBehaviour
{
    [SerializeField] GameObject[] framesShow = null;
    private int indexToShow = 0;
    private float eslcap = 0;

    void Start()
    {
        foreach (var i in framesShow)
        {
            i.SetActive(false);
        }
        framesShow[indexToShow].SetActive(true);
    }
    void FixedUpdate()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))&&eslcap > 0.8f)
        {
            if (indexToShow < framesShow.Length)
            {
                FadeOut();
                Invoke("FadeIn", 0.8f);
            }
            eslcap = 0f;
        }
        eslcap+=Time.deltaTime;
    }
    void FadeOut()
    {
        FindObjectOfType<FadeController>().DoFadeOut();
    }
    void FadeIn()
    {
        FindObjectOfType<FadeController>().DoFadeIn();
        indexToShow = indexToShow+1;
        if (indexToShow < framesShow.Length)
        {
            foreach (var i in framesShow)
            {
                i.SetActive(false);
            }
            framesShow[indexToShow].SetActive(true);
        }
        else
        {
            GameCore.m_GameContrller.GameStart();
            FindObjectOfType<Map1>().PlayThemeSound(1);
            FindObjectOfType<Map1>().ShowQuest();
            Destroy(this.gameObject);
        }
    }
}
