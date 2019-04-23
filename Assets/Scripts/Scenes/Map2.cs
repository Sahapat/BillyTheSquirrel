using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public static class Temp
{
    public static int maxHP = 100;
    public static int maxSP = 100;

    public static int Coin = 0;
}
public class Map2 : MonoBehaviour
{
    bool isSet = false;
    [SerializeField] GameObject questObject = null;
    [SerializeField] TextMeshProUGUI textQuest = null;
    [SerializeField] string[] quests = null;
    [SerializeField] AudioSource shareAudio = null;
    [SerializeField] AudioClip clip;
    public int currentQuest = 0;
    void Start()
    {
        questObject.SetActive(true);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            GameCore.m_GameContrller.ClientPlayerTarget.transform.position = new Vector3(132.03f,54.04f,38.95f);
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            foreach(var i in FindObjectOfType<Map2Quest2>().chiefObject)
            {
                i.GetComponent<Enemy>().TakeDamage(180);
            }
        }
        if(Input.GetKeyDown(KeyCode.F6))
        {
            GameCore.m_Main.LoadGameScene("Finish");
        }
        if (!isSet)
        {
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterHP.SetMaxHP(Temp.maxHP);
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterStemina.SetMaxSP(Temp.maxSP);
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin = Temp.Coin;

            GameCore.m_uiHandler.UpdateHPMax();
            GameCore.m_uiHandler.UpdateSPMax();
            GameCore.m_GameContrller.GameStart();
            isSet = true;
        }
        if (GameCore.m_cameraController.enabled)
        {
            if (questObject.activeSelf)
            {
                textQuest.text = quests[currentQuest];
            }
        }
    }
    public void ShowQuest()
    {
        questObject.SetActive(true);
    }
    public void SetCurrentQuest(int index)
    {
        currentQuest = index;
        shareAudio.PlayOneShot(clip);
    }
}
