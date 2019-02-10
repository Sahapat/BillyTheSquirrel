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
    }
    void OnResetHP()
    {
        UpdateHPBar(GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.HP);
    }
    void UpdateHPBar(int value)
    {
        int assignValue = Mathf.Clamp(value,0,100);
        m_HPSlider.value = assignValue;
        print("in");
    }
}
