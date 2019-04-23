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
    [SerializeField] GameObject questHUB = null;

    public int currentItemIndex { get; private set; } = -1;

    private Player m_player = null;
    void Start()
    {
        m_player = GameCore.m_GameContrller.ClientPlayerTarget;
        m_player.CharacterHP.OnHPChanged += UpdateHPBar;
        m_player.CharacterStemina.OnSteminachange += UpdateSPBar;
        m_player.CharacterCoin.OnCoinAdd += UpdateCoinTxt;
        m_player.CharacterCoin.OnCoinRemove += UpdateCoinTxt;

        UpdateHPBar(m_player.CharacterHP.HP);
        UpdateSPBar(m_player.CharacterStemina.SP);
        UpdateHPMax();
        UpdateSPMax();
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
        questHUB.SetActive(false);
    }
    public void CloseInventory()
    {
        InventoryHub.CloseInventory();
        GameCore.m_cameraController.SetCameraNormalState();
        GameCore.m_CursorController.DisableCursor();
        if(GameCore.m_GameContrller.isGameStart)
        {
            questHUB.SetActive(true);
        }
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
    public void UpdateHPMax()
    {
        /* var hpRect = m_HPSlider.GetComponent<RectTransform>();
        var assignWidth = m_player.CharacterHP.MaxHP * 4;
        var assignPosX = 400-assignWidth;
        assignPosX/=2;

        assignPosX = Mathf.Abs(assignPosX);

        if(m_player.CharacterHP.MaxHP < 100)
        {
            hpRect.sizeDelta = new Vector2(assignWidth,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(hpRect.anchoredPosition.x-assignPosX,hpRect.anchoredPosition.y);
        }
        else
        {
            hpRect.sizeDelta = new Vector2(assignWidth,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(hpRect.anchoredPosition.x+assignPosX,hpRect.anchoredPosition.y);
        } */
        var hpRect = m_HPSlider.GetComponent<RectTransform>();
        switch(m_player.CharacterHP.MaxHP)
        {
            case 100:
            hpRect.sizeDelta = new Vector2(400,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-260,hpRect.anchoredPosition.y);
            break;
            case 120:
            hpRect.sizeDelta = new Vector2(480,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-220,hpRect.anchoredPosition.y);
            break;
            case 140:
            hpRect.sizeDelta = new Vector2(560,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-180,hpRect.anchoredPosition.y);
            break;
            case 160:
            hpRect.sizeDelta = new Vector2(640,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-140,hpRect.anchoredPosition.y);
            break;
            case 180:
            hpRect.sizeDelta = new Vector2(720,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-100,hpRect.anchoredPosition.y);
            break;
            case 200:
            hpRect.sizeDelta = new Vector2(800,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(-60,hpRect.anchoredPosition.y);
            break;
        }
    }
    public void UpdateSPMax()
    {
        /* var hpRect = m_SPSlider.GetComponent<RectTransform>();
        var assignWidth = m_player.CharacterStemina.MaxSP * 4;
        var assignPosX = 400-assignWidth;
        assignPosX/=2;

        if(m_player.CharacterStemina.MaxSP < 100)
        {
            hpRect.sizeDelta = new Vector2(assignWidth,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(hpRect.anchoredPosition.x+assignPosX,hpRect.anchoredPosition.y);
        }
        else
        {
            hpRect.sizeDelta = new Vector2(assignWidth,hpRect.rect.height);
            hpRect.anchoredPosition = new Vector2(hpRect.anchoredPosition.x-assignPosX,hpRect.anchoredPosition.y);
        } */

        var spRect = m_SPSlider.GetComponent<RectTransform>();
        switch(m_player.CharacterStemina.MaxSP)
        {
            case 100:
            spRect.sizeDelta = new Vector2(400,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-260,spRect.anchoredPosition.y);
            break;
            case 120:
            spRect.sizeDelta = new Vector2(480,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-220,spRect.anchoredPosition.y);
            break;
            case 140:
            spRect.sizeDelta = new Vector2(560,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-180,spRect.anchoredPosition.y);
            break;
            case 160:
            spRect.sizeDelta = new Vector2(640,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-140,spRect.anchoredPosition.y);
            break;
            case 180:
            spRect.sizeDelta = new Vector2(720,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-100,spRect.anchoredPosition.y);
            break;
            case 200:
            spRect.sizeDelta = new Vector2(800,spRect.rect.height);
            spRect.anchoredPosition = new Vector2(-60,spRect.anchoredPosition.y);
            break;
        }
    }
    void UpdateHPBar(int value)
    {
        int assignValue = (value * 100) / m_player.CharacterHP.MaxHP;
        m_HPSlider.value = assignValue;
    }
    void UpdateSPBar(int value)
    {
        int assignValue = (value * 100) / m_player.CharacterHP.MaxHP;
        m_SPSlider.value = assignValue;
    }
    void UpdateCoinTxt(int value)
    {
        m_CoinText.text = m_player.CharacterCoin._Coin.ToString();
    }
}
