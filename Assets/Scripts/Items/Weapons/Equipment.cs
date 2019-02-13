using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour,ICollectable
{
    public enum EquipmentType
    {
        WEAPON,
        SHIELD
    };

    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]EquipmentType _equipmentType = EquipmentType.WEAPON;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _discription = string.Empty;
    [SerializeField,Range(1,2)]byte _UsedSlot = 1;
    [SerializeField]Vector3 _HoldingPos = Vector3.zero;
    [SerializeField]NormalHit normalHit = null;
    [SerializeField]NormalHit heavyHit = null;
    public Vector3 HoldingPos{get{return _HoldingPos;}}
    public ItemType itemType{get{return _itemType;}}
    public EquipmentType equipmentType{get{return _equipmentType;}}
    public Sprite Icon{get{return _Icon;}}
    public string discription{get{return _discription;}}

    private BoxCollider m_boxcolider = null;
    private Rigidbody m_rigidbody = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public GameObject PickUp()
    {
        m_boxcolider.isTrigger = true;
        m_rigidbody.isKinematic = true;
        this.gameObject.layer = 0;
        return this.gameObject;
    }
    public void NormalAttack()
    {
        normalHit.ActiveHit();
    }
    public void HeavyAttack()
    {
        heavyHit.ActiveHit();
    }
}
