using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Slider m_HPSlider = null;
    [SerializeField] Slider m_SPSlider = null;
    [SerializeField] TextMeshProUGUI m_LevelText = null;
    [SerializeField] TextMeshProUGUI m_CoinText = null;
    [SerializeField] InventoryUIController InventoryHub = null;
    [SerializeField] Image currentItemShow = null;
    [SerializeField] Sprite noneCurrentItem = null;

    [Header("Object UI")]
    [SerializeField] GameObject statusHUB = null;
    [SerializeField] GameObject GameOverHUB = null;

    public int currentItemIndex {get;private set;} = -1;
    void Start()
    {
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.OnHPChanged += UpdateHPBar;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.OnResetHP += OnResetHP;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.OnSteminaReset += OnResetSP;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.OnSteminachange += UpdateSPBar;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterCoin.OnCoinAdd += UpdateCoinTxt;
        GameCore.m_GameContrller.GetClientPlayerTarget().CharacterCoin.OnCoinRemove += UpdateCoinTxt;
        OnResetHP();
        OnResetSP();
    }
    public void ShowGameOver()
    {
        statusHUB.SetActive(false);
        GameOverHUB.SetActive(true);
        GameCore.m_cameraController.SetCameraState(CameraController.CameraState.INVENTORY);
        GameCore.m_CursorController.SetCursorInInventoryMode();
    }
    public void CloseGameOver()
    {
        statusHUB.SetActive(true);
        GameOverHUB.SetActive(false);
    }
    public void OpenInventory()
    {
        InventoryHub.OpenInventory();
    }
    public void CloseInventory()
    {
        InventoryHub.CloseInventory();
    }
    public bool GetInventoryStatus()
    {
        return InventoryHub.inventoryStatus;
    }
    public void RemoveCurrentItem()
    {
        currentItemIndex = -1;
        currentItemShow.sprite = noneCurrentItem;
    }
    public void SetCurrentItem(int index, Sprite icon)
    {
        if (icon == null)
        {
            RemoveCurrentItem();
        }
        else
        {
            currentItemIndex = index;
            currentItemShow.sprite = icon;
        }
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
        int assignValue = Mathf.Clamp(value, 0, GameCore.m_GameContrller.GetClientPlayerTarget().CharacterHP.MaxHP);
        m_HPSlider.value = assignValue;
    }
    void UpdateSPBar(int value)
    {
        int assignValue = Mathf.Clamp(value, 0, GameCore.m_GameContrller.GetClientPlayerTarget().CharacterStemina.MaxSP);
        m_SPSlider.value = assignValue;
    }
    void UpdateCoinTxt(int value)
    {
        m_CoinText.text = GameCore.m_GameContrller.GetClientPlayerTarget().CharacterCoin._Coin.ToString();
    }
}
