using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private BoxCollider m_boxColider2D;
    private Rigidbody m_rigidbody;
    void Awake()
    {
        m_boxColider2D = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    public GameObject PickObjUp()
    {
        m_boxColider2D.enabled = false;
        m_rigidbody.isKinematic = true;
        return this.transform.gameObject;
    }
}
