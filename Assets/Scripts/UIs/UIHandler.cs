using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]Slider m_HPSlider = null;
    [SerializeField]Slider m_SPSlider = null;
    [SerializeField]Text m_LevelText = null;
    [SerializeField]Text m_CoinText = null;

    [SerializeField]GameObject InventoryHub = null;

    public void UpdateHPBar(int value)
    {
        int assignValue = Mathf.Clamp(value,0,100);
        m_HPSlider.value = assignValue;
    }
    public void UpdateSpBar(int value)
    {
        int assignValue = Mathf.Clamp(value,0,100);
        m_SPSlider.value = assignValue;
    }
    public void UpdateLevelTxt(string txt)
    {
        m_LevelText.text = txt;
    }
    public void UpdateCoinTxt(int value)
    {
        m_CoinText.text = value.ToString();
    }
    public void SwitchActiveInventoryHUB()
    {
        if(InventoryHub.activeSelf)
        {
            CloseInventoryHUB();
        }
        else
        {
            OpenInventoryHUB();
        }
    }
    void OpenInventoryHUB()
    {
        InventoryHub.SetActive(true);
        GameCore.m_CursorController.CursorInUiMode();
    }
    void CloseInventoryHUB()
    {
        InventoryHub.SetActive(false);
        GameCore.m_CursorController.CursorInGameMode();
    }
}
