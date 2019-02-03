using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Item
{
    public enum ItemType
    {
        WEAPON,
        ITEM
    };
    public string discription;
};
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
    }
    void AddWeapon(Item weaponIn)
    {
        /* if(weapon_item.Count > maxWeapon)
        {
        } */
    }
}
