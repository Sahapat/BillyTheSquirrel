﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    SHIELD_AND_SWORD,
    AXE,
    SPEAR,
    GREAT_SWORD,
    NONE
};

public class BaseWeapon: MonoBehaviour,ICollectable,IPopable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]WeaponType _weaponType = WeaponType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _headerName = string.Empty;
    [SerializeField]string _description = string.Empty;
    [SerializeField]Vector3 _HoldingPos = Vector3.zero;
    [SerializeField]bool isPickUp = false;

    public WeaponType weaponType{get{return _weaponType;}}
    public Vector3 HoldingPos{get{return _HoldingPos;}}
    public ItemType itemType{get{return _itemType;}}
    public Sprite Icon{get{return _Icon;}}
    public string description{get{return _description;}}
    public string headerName{get{return _headerName;}}
    public HitManager hitSystemManager{get;private set;}

    /* bool isPopOut = false; */

    void Awake()
    {
        hitSystemManager = GetComponent<HitManager>();
    }
    void FixedUpdate()
    {
        /* if(isPopOut)
        {
            var hitGround = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Ground"));
            if (hitGround.Length > 0)
            {
                m_rigidbody.isKinematic = true;
                isPopOut = false;
            }
        } */
    }
    void Start()
    {
        if(isPickUp)SetPickUp();
    }
    public GameObject PickUp()
    {
        isPickUp = true;
        SetPickUp();
        return this.gameObject;
    }
    public void Discard()
    {
        this.transform.parent = null;
        isPickUp = false;
        /* m_boxcolider.enabled =true;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false; */
    }
    void SetPickUp()
    {
        /* m_boxcolider.enabled = false;
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true; */
    }

    public void PopOut(float xForce, float zForce, float forceToAdd)
    {
        /* isPopOut = true;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(new Vector3(xForce,Vector3.up.y,zForce) * forceToAdd, ForceMode.Impulse); */
    }
    public int GetNormalSteminaDeplete(int index)
    {
        return hitSystemManager.GetNormalHitSteminaDeplete(index);
    }
    public int GetHeavySteminaDeplete()
    {
        return hitSystemManager.GetHeavyHitSteminaDeplete();
    }
    public void SetTargetLayer(LayerMask mask)
    {
        hitSystemManager.SetTargetLayer(mask);
    }
}