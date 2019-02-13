using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour, ICollectable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _discription = string.Empty;
    public ItemType itemType{get{return _itemType;}}
    public Sprite Icon{get{return _Icon;}}
    public string discription{get{return _discription;}}

    public GameObject PickUp()
    {
        OnPickUp();
        return this.gameObject;
    }
    protected virtual void OnPickUp()
    {
        return;
    }
}