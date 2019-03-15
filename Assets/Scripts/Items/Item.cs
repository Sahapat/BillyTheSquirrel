using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour, ICollectable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]ItemUsedType _itemUsedType = ItemUsedType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _headerName = string.Empty;
    [SerializeField]string _description = string.Empty;
    public ItemType itemType{get{return _itemType;}}
    public ItemUsedType itemUsedType{get{return _itemUsedType;}}
    public Sprite Icon{get{return _Icon;}}
    public string description{get{return _description;}}
    public string headerName {get{return _headerName;}}
    
    public GameObject PickUp()
    {
        OnPickUp();
        return this.gameObject;
    }
    public virtual void Use(Player player)
    {
        return;
    }
    public virtual void Use()
    {
        return;
    }
    protected virtual void OnPickUp()
    {
        return;
    }
}