using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    /* [SerializeField]int maxWeapon = 2;
    [SerializeField]int maxItem = 8;
 */
    private List<Item> weapon_item = null;
    private List<Item> item_item = null;

    void Awake()
    {
        weapon_item = new List<Item>();
        item_item =new List<Item>();
        
        GameCore.m_inventoryController.SetInventoryTarget(this);
    }
    void AddWeapon(Item weaponIn)
    {
        weapon_item.Add(weaponIn);
    }
    void AddItem(Item itemIn)
    {
        item_item.Add(itemIn);
    }
    public Item GetWeaponItem(int index)
    {
        if(weapon_item.Count < (index+1))
        {
            return Item.GetDefualtValue();
        }
        return weapon_item[index];
    }
    public Item GetItemItem(int index)
    {
        if(item_item.Count < (index+1))
        {
            return Item.GetDefualtValue();
        }
        return item_item[index];
    }
}
