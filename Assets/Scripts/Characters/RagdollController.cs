using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollsRigidbody = null;
    [SerializeField] Transform[] childForSwitchState = null;
    [SerializeField] Transform ActiveParent;
    [SerializeField] Transform InActiveParent;
    [SerializeField] int ActiveLayer = 0;
    [SerializeField] int InActiveLayer = 0;

    private Rigidbody m_parentRigidbody = null;
    void Awake()
    {
        m_parentRigidbody = GetComponent<Rigidbody>();
    }
    public void ActiveRagdoll()
    {
        foreach(Rigidbody m_rigid in ragdollsRigidbody)
        {
            m_rigid.isKinematic = false;
            m_rigid.useGravity = true;
            m_rigid.gameObject.layer = ActiveLayer;
        }
        foreach(Transform child in childForSwitchState)
        {
            child.parent = ActiveParent;
        }
        m_parentRigidbody.isKinematic = true;
        m_parentRigidbody.useGravity = false;
        this.gameObject.layer = InActiveLayer;
    }
    public void ActiveRagdoll(Vector3 velocity)
    {
        foreach(Rigidbody m_rigid in ragdollsRigidbody)
        {
            m_rigid.isKinematic = false;
            m_rigid.useGravity = true;
            m_rigid.gameObject.layer = ActiveLayer;
            m_rigid.velocity = velocity;
        }
        foreach(Transform child in childForSwitchState)
        {
            child.parent = ActiveParent;
        }
        m_parentRigidbody.isKinematic = true;
        m_parentRigidbody.useGravity = false;
        this.gameObject.layer = InActiveLayer;
    }
    public void InActiveRagdoll()
    {
        foreach(Rigidbody m_rigid in ragdollsRigidbody)
        {
            m_rigid.isKinematic = true;
            m_rigid.useGravity = false;
            m_rigid.gameObject.layer = InActiveLayer;
        }
        foreach(Transform child in childForSwitchState)
        {
            child.parent = InActiveParent;
        }
        m_parentRigidbody.isKinematic = false;
        m_parentRigidbody.useGravity = true;
        this.gameObject.layer = ActiveLayer;
    }
}
