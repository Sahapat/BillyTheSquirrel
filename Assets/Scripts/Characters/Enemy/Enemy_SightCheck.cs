using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SightCheck : MonoBehaviour
{
    [SerializeField]LayerMask EnemyLayer = 0;

    public GameObject EnemyInSight{get;private set;}
    private SphereCollider m_sphereCollider =null;
    void Awake()
    {
        m_sphereCollider = GetComponent<SphereCollider>();
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapSphere(m_sphereCollider,EnemyLayer);
        if(hitInfo.Length > 0)
        {
            EnemyInSight = hitInfo[0].gameObject;
        }
        else
        {
            EnemyInSight = null;
        }
    }
}
