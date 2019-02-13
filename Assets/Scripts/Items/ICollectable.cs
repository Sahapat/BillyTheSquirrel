using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    EQUIPMENT,
    ITEM,
    NONE
};
public interface ICollectable
{
    ItemType itemType{get;}
    string discription{get;}
    Sprite Icon{get;}
    GameObject PickUp();
}
