using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField]GameObject inventoryHubObj = null;

    private ItemSlot[] m_itemSlot;
    private ItemSlot[] m_weaponSlot;
    private Inventory inventoryTarget = null;
    void Awake()
    {
        inventoryHubObj.SetActive(true);
        var itemSlots = GameObject.FindGameObjectsWithTag("SlotItem");
        var weaponSlots = GameObject.FindGameObjectsWithTag("SlotWeapon");
        m_itemSlot = new ItemSlot[itemSlots.Length];
        m_weaponSlot = new ItemSlot[weaponSlots.Length];

        for(int i =0;i<itemSlots.Length;i++)
        {
            m_itemSlot[i] = itemSlots[i].GetComponent<ItemSlot>();
        }
        for(int i=0;i<weaponSlots.Length;i++)
        {
            m_weaponSlot[i] = weaponSlots[i].GetComponent<ItemSlot>();
        }
        inventoryHubObj.SetActive(false);
    }
    public void SetInventoryTarget(Inventory target)
    {
        inventoryTarget = target;        
    }
    public void InventorySwitch()
    {
        if(inventoryHubObj.activeSelf)
        {
            OnClose();
        }
        else
        {
            OnOpen();
        }
        inventoryHubObj.SetActive(!inventoryHubObj.activeSelf);
    }
    public void OnOpen()
    {
        LoadItemList();
    }
    public void OnClose()
    {
        return;
    }
    void LoadItemList()
    {
        for(int i=0;i<m_weaponSlot.Length;i++)
        {
            if(inventoryTarget.GetWeaponItem(i).itemType != Item.GetDefualtValue().itemType)
            {
                m_weaponSlot[i].SetImage(inventoryTarget.GetWeaponItem(i));
            }
            else
            {
                m_weaponSlot[i].Clear();
            }
        }

        for(int i=0;i<m_itemSlot.Length;i++)
        {
            if(inventoryTarget.GetWeaponItem(i).itemType != Item.GetDefualtValue().itemType)
            {
                m_itemSlot[i].SetImage(inventoryTarget.GetWeaponItem(i));
            }
            else
            {
                m_itemSlot[i].Clear();
            }
        }
    }
}
