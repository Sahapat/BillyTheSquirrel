using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory
{
    public struct ItemInInventory
    {
        public ItemType itemType;
        public Sprite Icon;
        public string description;

        public void SetDefualtValue()
        {
            itemType = ItemType.NONE;
            Icon = null;
            description = string.Empty;
        }
        public bool CheckIsEmpty()
        {
            var check1 = (itemType == ItemType.NONE)?true:false;
            var check2 = (Icon == null)?true:false;
            var check3 = (description == string.Empty)?true:false;

            return check1 && check2&& check3;
        }
    }
    public delegate void _FuncICollecable(ICollectable itemAdded);
    public delegate void _FuncInt(int removeIndex);
    public event _FuncICollecable OnItemAdd;
    public event _FuncInt OnItemRemove;

    private ItemInInventory[] _ItemInInventory;
    private int addedIndex = 0;
    private int recentlyRemoveIndex = -1;

    public Inventory(int size)
    {
        _ItemInInventory = new ItemInInventory[size];
    }
    public ItemInInventory GetItem(int index)
    {
        return _ItemInInventory[index];
    }

    public bool AddItem(ICollectable itemAdded)
    {
        int indexToAdd = 0;

        if(recentlyRemoveIndex != -1)
        {
            indexToAdd = recentlyRemoveIndex;
            recentlyRemoveIndex= -1;    
        }
        else if(addedIndex != _ItemInInventory.Length-1)
        {
            indexToAdd = addedIndex;
            addedIndex++;
        }
        else
        {
            return false;
        }

        _ItemInInventory[indexToAdd].itemType = itemAdded.itemType;
        _ItemInInventory[indexToAdd].Icon = itemAdded.Icon;
        _ItemInInventory[indexToAdd].description = itemAdded.discription;
        OnItemAdd?.Invoke(itemAdded);
        return true;
    }
    public bool RemoveItem(int index)
    {
        if(_ItemInInventory[index].CheckIsEmpty())
        {
            return false;
        }
        else
        {
            recentlyRemoveIndex = index;
            OnItemRemove?.Invoke(index);
            return true;
        }
    }

}
