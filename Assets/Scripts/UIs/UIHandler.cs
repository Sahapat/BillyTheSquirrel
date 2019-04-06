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
    //[SerializeField] TextMeshProUGUI m_LevelText = null;
    [SerializeField] TextMeshProUGUI m_CoinText = null;
    [SerializeField] InventoryUIController InventoryHub = null;
    [SerializeField] Image currentItemShow = null;
    [SerializeField] Sprite noneCurrentItem = null;

    [Header("Object UI")]
    [SerializeField] GameObject statusHUB = null;
    [SerializeField] GameObject GameOverHUB = null;
    [SerializeField] GameObject MarketHUB = null;

    public int currentItemIndex {get;private set;} = -1;

    private Player m_player = null;
    void Start()
    {
        m_player = GameCore.m_GameContrller.GetClientPlayerTarget();
        m_player.CharacterHP.OnHPChanged += UpdateHPBar;
        m_player.CharacterHP.OnResetHP += OnResetHP;
        m_player.CharacterStemina.OnSteminaReset += OnResetSP;
        m_player.CharacterStemina.OnSteminachange += UpdateSPBar;
        m_player.CharacterCoin.OnCoinAdd += UpdateCoinTxt;
        m_player.CharacterCoin.OnCoinRemove += UpdateCoinTxt;
        
        OnResetHP();
        OnResetSP();
        UpdateEquipmentSlot();
    }
    public void CloseMarket()
    {
        MarketHUB.SetActive(false);
    }
    public void OpenMarket()
    {
        MarketHUB.SetActive(true);
    }
    public void ShowGameOver()
    {
        statusHUB.SetActive(false);
        GameOverHUB.SetActive(true);
        GameCore.m_cameraController.SetCameraUI_ManageState();
        GameCore.m_CursorController.EnableCursor();
    }
    public void CloseGameOver()
    {
        statusHUB.SetActive(true);
        GameOverHUB.SetActive(false);
    }
    public void OpenInventory()
    {
        InventoryHub.OpenInventory();
        GameCore.m_cameraController.SetCameraUI_ManageState();
        GameCore.m_CursorController.EnableCursor();
    }
    public void CloseInventory()
    {
        InventoryHub.CloseInventory();
        GameCore.m_cameraController.SetCameraNormalState();
        GameCore.m_CursorController.DisableCursor();
    }
    public bool GetInventoryStatus()
    {
        return InventoryHub.inventoryStatus;
    }
    public void UpdateEquipmentSlot()
    {
        InventoryHub.UpdateWeapon();
        InventoryHub.UpdateShield();
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
        UpdateHPBar(m_player.CharacterHP.HP);
    }
    void OnResetSP()
    {
        UpdateSPBar(m_player.CharacterStemina.SP);
    }
    void UpdateHPBar(int value)
    {
        int assignValue = (value * 100)/m_player.CharacterHP.MaxHP;
        m_HPSlider.value = assignValue;
    }
    void UpdateSPBar(int value)
    {
        int assignValue = (value * 100)/m_player.CharacterHP.MaxHP;
        m_SPSlider.value = assignValue;
    }
    void UpdateCoinTxt(int value)
    {
        m_CoinText.text = m_player.CharacterCoin._Coin.ToString();
    }
}
