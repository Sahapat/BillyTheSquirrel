using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] GameObject TargetDescriptionObject = null;
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
        GameCore.m_GameContrller.GetClientPlayerTarget().ItemInventory.OnItemAdded += UpdateItem;
        GameCore.m_GameContrller.GetClientPlayerTarget().ItemInventory.OnItemRemove += UpdateItem;
        m_player = GameCore.m_GameContrller.GetClientPlayerTarget();
    }
    void Update()
    {
        if (inventoryStatus && !inputTrigger)
        {
            var directionX = Input.GetAxis("Horizontal");
            var directionY = Input.GetAxis("Vertical");

            //print($"DirectionX {directionX}  DirectionY {directionY}");
            bool isHorizontalDirection = Mathf.Abs(directionX) > Mathf.Abs(directionY);
            if (isHorizontalDirection && Mathf.Abs(directionX) > 0.5f)
            {
                inputTrigger = true;
                selectedSlotIndex = (directionX > 0) ? selectedSlotIndex + 1 : selectedSlotIndex - 1;
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, slotUIControllers.Length - 1);
                ResetAndUpdateSelected();
                Invoke("SetCanInputAgain", 0.15f);
            }
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                if (enterSelected != selectedSlotIndex)
                {
                    TargetDescriptionObject.SetActive(true);
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
                    TargetDescriptionObject.SetActive(false);
                }
            }
        }
    }
    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        ResetAndUpdateSelected();
        SetCanInputAgain();
    }
    public void CloseInventory()
    {
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
        inventoryStatus = false;
        selectedSlotIndex = 0;
    }
    public void OpenInventory()
    {
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
        TargetDescriptionObject.SetActive(false);
        inventoryStatus = true;
        ResetAndUpdateSelected();
        SetCanInputAgain();
    }
    public void UpdateWeapon()
    {
        slotUIControllers[0].AddItemToSlot(m_player.WeaponInventory);
    }
    public void UpdateShield()
    {
        slotUIControllers[1].AddItemToSlot(m_player.ShieldInvetory);
    }
    public void UpdateItem()
    {
        for (int i = 0; i < m_player.ItemInventory.items.Length; i++)
        {
            slotUIControllers[i + 2].AddItemToSlot(m_player.ItemInventory.items[i]);
        }
    }
    void ResetAndUpdateSelected()
    {
        foreach (SlotUIController slot in slotUIControllers)
        {
            slot.SetUnSelectedSlot();
        }
        slotUIControllers[selectedSlotIndex].SetSelectedSlot();

    }
    void SetCanInputAgain()
    {
        inputTrigger = false;
    }
}
