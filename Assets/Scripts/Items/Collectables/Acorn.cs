using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour,IPopable
{
    [SerializeField] int coinValue = 1;
    [SerializeField] Vector3 RotateDirection = Vector3.zero;
    [SerializeField] float RotateSpeed = 10;
    private BoxCollider m_Boxcolider = null;
    private Rigidbody m_rigidBody = null;
    private bool isPopOut = false;
    void Awake()
    {
        m_Boxcolider = GetComponent<BoxCollider>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.isKinematic = true;
    }
    void FixedUpdate()
    {
        if (isPopOut)
        {
            var hitGround = PhysicsExtensions.OverlapBox(m_Boxcolider, LayerMask.GetMask("Ground"));
            if (hitGround.Length > 0)
            {
                m_rigidBody.isKinematic = true;
                isPopOut = false;
            }
        }
        else
        {
            transform.Rotate(RotateDirection * Time.deltaTime * RotateSpeed, Space.World);
            var hitInfo = PhysicsExtensions.OverlapBox(m_Boxcolider, LayerMask.GetMask("Character"));
            if (hitInfo.Length > 0)
            {
                GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.AddCoin(coinValue);
                Destroy(this.gameObject);
            }
        }
    }
    public void PopOut(float xForce,float zForce,float forceToAdd)
    {
        m_rigidBody.isKinematic = false;
        isPopOut = true;
        m_rigidBody.AddForce(new Vector3(xForce,Vector3.up.y,zForce) * forceToAdd, ForceMode.Impulse);
    }
}
