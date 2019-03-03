using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSword: MonoBehaviour,ICollectable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _discription = string.Empty;
    [SerializeField,Range(1,2)]byte _UsedSlot = 1;
    [SerializeField]Vector3 _HoldingPos = Vector3.zero;
    [SerializeField]bool isPickUp = false;

    public Vector3 HoldingPos{get{return _HoldingPos;}}
    public ItemType itemType{get{return _itemType;}}
    public Sprite Icon{get{return _Icon;}}
    public string discription{get{return _discription;}}
    public HitManager hitSystemManager{get;private set;}

    private BoxCollider m_boxcolider = null;
    private Rigidbody m_rigidbody = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
        hitSystemManager = GetComponent<HitManager>();
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
        isPickUp = false;
        m_boxcolider.enabled =true;
        m_rigidbody.isKinematic = false;
        this.gameObject.layer = LayerMask.GetMask("PickUp");
    }
    void SetPickUp()
    {
        m_boxcolider.enabled = false;
        m_rigidbody.isKinematic = true;
        this.gameObject.layer = 0;
    }
}
