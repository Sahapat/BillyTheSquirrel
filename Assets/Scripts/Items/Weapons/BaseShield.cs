using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShield : MonoBehaviour,ICollectable,IPopable
{
    [SerializeField]ItemType _itemType = ItemType.NONE;
    [SerializeField]Sprite _Icon = null;
    [SerializeField]string _headerName = string.Empty;
    [SerializeField]string _description = string.Empty;
    [SerializeField]Vector3 _HoldingPos = Vector3.zero;
    [SerializeField]bool isPickUp = false;

    public Vector3 HoldingPos{get{return _HoldingPos;}}
    public ItemType itemType{get{return _itemType;}}
    public Sprite Icon{get{return _Icon;}}
    public string description{get{return _description;}}
    public string headerName{get{return _headerName;}}

    private BoxCollider m_boxcolider = null;
    private Rigidbody m_rigidbody = null;
    bool isPopOut = false;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        if(isPickUp)SetPickUp();
    }
    void FixedUpdate()
    {
        if(isPopOut)
        {
            var hitGround = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Ground"));
            if (hitGround.Length > 0)
            {
                m_rigidbody.isKinematic = true;
                isPopOut = false;
            }
        }
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
        m_boxcolider.enabled =true;
        m_rigidbody.isKinematic = false;
        m_rigidbody.useGravity = true;
    }
    void SetPickUp()
    {
        m_boxcolider.enabled = false;
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
    }

    public void PopOut(float xForce, float zForce, float forceToAdd)
    {
        isPopOut = true;
        isPickUp =false;
        m_rigidbody.isKinematic = false;
        m_rigidbody.useGravity = true;
        m_rigidbody.AddForce(new Vector3(xForce,Vector3.up.y,zForce) * forceToAdd, ForceMode.Impulse);
    }
}
