using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory
{
    public delegate void _Func();
    public event _Func OnItemAdded;
    public event _Func OnItemRemove;

    public ICollectable[] items{get;private set;}

    public GameObject[] itemInEndPoint{get;private set;}
    public bool isFull{get;private set;}
    int addedIndex = 0;
    int specialAddIndex = -1;
    int maxSlot = 0;
    public Inventory(int maxSlot)
    {
        items = new ICollectable[maxSlot];
        itemInEndPoint = new GameObject[maxSlot];
        this.maxSlot = maxSlot;
    }
    public void AddItem(GameObject itemAdded,Transform endPoint)
    {
        int indexToAdd = 0;

        if(specialAddIndex != -1)
        {
            indexToAdd = specialAddIndex;
            specialAddIndex = -1;
        }
        else
        {
            indexToAdd = addedIndex;
        }
        
        items[indexToAdd] = itemAdded.GetComponent<ICollectable>();
        itemAdded.transform.parent = endPoint;
        itemAdded.transform.localPosition = Vector3.zero;
        itemAdded.transform.localRotation = Quaternion.identity;
        itemInEndPoint[indexToAdd] = itemAdded;
        
        addedIndex = (addedIndex+1 > maxSlot)?-1:addedIndex+1;

        if(addedIndex == -1 && specialAddIndex == -1)
        {
            isFull =true;
        }
        OnItemAdded?.Invoke();
    }
    public GameObject RemoveItem(int index)
    {
        items[index] = null;
        specialAddIndex = index;
        OnItemRemove?.Invoke();
        return itemInEndPoint[index];
    }
}
