using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]LayerMask groundMask = 0;
    private BoxCollider m_boxcolider = null;
    public bool isOnGround{get;private set;} = false;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void FixedUpdate()
    {
        var groundHit = PhysicsExtensions.OverlapBox(m_boxcolider,groundMask);
        isOnGround = (groundHit.Length > 0)?true:false;
    }
}
