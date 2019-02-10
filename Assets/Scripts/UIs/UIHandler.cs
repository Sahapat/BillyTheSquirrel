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

    void Start()
    {
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.OnHPChanged += UpdateHPBar;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.OnResetHP += OnResetHP;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.OnSteminaReset += OnResetSP;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.OnSteminachange += UpdateSPBar;
    }
    void OnResetHP()
    {
        UpdateHPBar(GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.HP);
    }
    void OnResetSP()
    {
        UpdateSPBar(GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.SP);
    }
    void UpdateHPBar(int value)
    {
        int assignValue = Mathf.Clamp(value,0,GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.MaxHP);
        m_HPSlider.value = assignValue;
    }
    void UpdateSPBar(int value)
    {
        int assignValue = Mathf.Clamp(value,0,GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.MaxSP);
        m_SPSlider.value = assignValue;
    }
}
