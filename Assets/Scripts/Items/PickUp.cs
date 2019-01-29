using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Item
{
    public enum ItemType
    {
        WEAPON,
        ITEM,
        NONE
    };
    public Sprite midmapIcon;
    public ItemType itemType;
    public string discription;

    public static Item GetDefualtValue()
    {
        var temp = new Item();
        temp.itemType = ItemType.NONE;
        temp.discription = string.Empty;
        temp.midmapIcon = null;

        return temp;
    }
};
public class PickUp : MonoBehaviour
{
    [SerializeField]Item itemData = Item.GetDefualtValue();
    private BoxCollider m_boxColider2D;
    private Rigidbody m_rigidbody;
    void Awake()
    {
        m_boxColider2D = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    public GameObject PickObjUp()
    {
        m_boxColider2D.enabled = false;
        m_rigidbody.isKinematic = true;
        return this.transform.gameObject;
    }
    public Item GetItemData()
    {
        return itemData;
    }
}
