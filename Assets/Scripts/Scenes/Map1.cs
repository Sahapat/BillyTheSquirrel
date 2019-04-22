using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Map1 : MonoBehaviour
{
    [SerializeField] AudioSource themeSource = null;
    [SerializeField] AudioClip[] ThemeSounds = null;
    [SerializeField] GameObject questObject = null;
    [SerializeField] TextMeshProUGUI textQuest = null;
    [SerializeField] string[] quests = null;

    public int currentQuest = 0;
    void Start()
    {
        PlayThemeSound(0);
        questObject.SetActive(false);
    }
    void Update()
    {
        if (GameCore.m_cameraController.enabled)
        {
            if (questObject.activeSelf)
            {
                textQuest.text = quests[currentQuest];
            }
        }
    }
    public void PlayThemeSound(int index)
    {
        themeSource.clip = ThemeSounds[index];
        themeSource.Play();
    }
    public void SetCurrentQuest(int index)
    {
        currentQuest = index;
    }
    public void ShowQuest()
    {
        questObject.SetActive(true);
    }
}
