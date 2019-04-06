using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item,IPopable
{
    Rigidbody m_rigidBody = null;
    BoxCollider m_boxcolider = null;
    bool isPopOut = false;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidBody.isKinematic = true;
    }
    void Update()
    {
        if (isPopOut)
        {
            var hitGround = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Ground"));
            if (hitGround.Length > 0)
            {
                m_rigidBody.isKinematic = true;
                isPopOut = false;
            }
        }
    }
    public virtual void PopOut(float xForce, float zForce, float forceToAdd)
    {
        m_rigidBody.isKinematic = false;
        isPopOut = true;
        m_rigidBody.AddForce(new Vector3(xForce,Vector3.up.y,zForce) * forceToAdd, ForceMode.Impulse);
    }
}
