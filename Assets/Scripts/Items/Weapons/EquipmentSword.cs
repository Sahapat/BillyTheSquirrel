using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipmentSword : MonoBehaviour,ICollectable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _discription = string.Empty;
    [SerializeField,Range(1,2)]byte _UsedSlot = 1;
    [SerializeField]Vector3 _HoldingPos = Vector3.zero;
    [SerializeField]NormalHit normalHit = null;
    [SerializeField]NormalHit heavyHit = null;
    public Vector3 HoldingPos{get{return _HoldingPos;}}
    public ItemType itemType{get{return _itemType;}}
    public Sprite Icon{get{return _Icon;}}
    public string discription{get{return _discription;}}

    private BoxCollider m_boxcolider = null;
    private Rigidbody m_rigidbody = null;
    private TrailRenderer m_trail = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_trail = GetComponent<TrailRenderer>();
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
    }
    public void HeavyAttack()
    {
    }
}
