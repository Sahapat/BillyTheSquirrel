using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] GameObject targetDescriptionObject = null;
    [SerializeField] SlotUIController[] slotUIControllers = null;

    int selectedSlotIndex = 0;
    int enterSelected = -1;
    Image[] inventoryImg;
    TextMeshProUGUI[] inventoryTxt;
    Button[] inventoryButton;

    public bool inventoryStatus { get; private set; }

    bool inputTrigger = false;

    private Player m_player = null;

    void Awake()
    {
        inventoryImg = GetComponentsInChildren<Image>();
        inventoryTxt = GetComponentsInChildren<TextMeshProUGUI>();
        inventoryButton = GetComponentsInChildren<Button>();
    }
    void Start()
    {
        m_player = GameCore.m_GameContrller.GetClientPlayerTarget();
        m_player.ItemInventory.OnItemAdded += UpdateUiInventory;
        m_player.ItemInventory.OnItemRemove += UpdateUiInventory;
    }
    void Update()
    {
        #region Joystick selection part
        /* if (inventoryStatus && !inputTrigger)
        {
            var directionX = Input.GetAxis("Horizontal");
            var directionY = Input.GetAxis("Vertical");
            bool isHorizontalDirection = Mathf.Abs(directionX) > Mathf.Abs(directionY);
            if (isHorizontalDirection && Mathf.Abs(directionX) > 0.5f)
            {
                inputTrigger = true;
                selectedSlotIndex = (directionX > 0) ? selectedSlotIndex + 1 : selectedSlotIndex - 1;
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, slotUIControllers.Length - 1);
                Invoke("SetCanInputAgain", 0.15f);
            }
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                if (enterSelected != selectedSlotIndex)
                {
                    targetDescriptionObject.SetActive(true);
                    enterSelected = selectedSlotIndex;
                    if (selectedSlotIndex > 1)
                    {
                        if (m_player.ItemInventory.items[selectedSlotIndex - 2] == null)
                        {
                            GameCore.m_uiHandler.SetCurrentItem(selectedSlotIndex,null);
                        }
                        else
                        {
                            GameCore.m_uiHandler.SetCurrentItem(selectedSlotIndex, m_player.ItemInventory.items[selectedSlotIndex - 2].Icon);
                        }
                    }
                }
                else
                {
                    targetDescriptionObject.SetActive(false);
                }
            }
        } */
        #endregion
    }
    public void CloseInventory()
    {
        inventoryStatus = false;
        foreach (Image img in inventoryImg)
        {
            img.enabled = false;
        }
        foreach (TextMeshProUGUI txt in inventoryTxt)
        {
            txt.enabled = false;
        }
        foreach (Button btn in inventoryButton)
        {
            btn.enabled = false;
        }
        foreach (SlotUIController slot in slotUIControllers)
        {
            slot.SetUnSelectedSlot();
        }
        selectedSlotIndex = 0;
    }
    public void OpenInventory()
    {
        inventoryStatus = true;
        foreach (Image img in inventoryImg)
        {
            img.enabled = true;
        }
        foreach (TextMeshProUGUI txt in inventoryTxt)
        {
            txt.enabled = true;
        }
        foreach (Button btn in inventoryButton)
        {
            btn.enabled = true;
        }
        targetDescriptionObject.SetActive(false);
    }
    public void UpdateWeapon()
    {
        slotUIControllers[0].AddItemToSlot(m_player.WeaponInventory);
    }
    public void UpdateShield()
    {
        slotUIControllers[1].AddItemToSlot(m_player.ShieldInvetory);
    }
    public void UpdateUiInventory()
    {
        for(int i=2;i<slotUIControllers.Length;i++)
        {
            slotUIControllers[i].AddItemToSlot(m_player.ItemInventory.items[i-2]);
        }
    }
}
