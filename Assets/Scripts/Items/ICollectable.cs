using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    EQUIPMENT,
    ITEM,
    NONE
};
public enum ItemUsedType
{
    STATUS_MODIFILER,
    NONE
};
public interface ICollectable
{
    ItemType itemType{get;}
    string description{get;}
    string headerName{get;}
    Sprite Icon{get;}
    GameObject PickUp();
}
public interface IPopable
{
    void PopOut(float xForce,float zForce,float forceToAdd);
}
