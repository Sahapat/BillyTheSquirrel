using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCheck : MonoBehaviour
{
    [SerializeField]LayerMask EnemyLayer = 0;

    public GameObject targetInSight{get;private set;}
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
            targetInSight = hitInfo[0].gameObject;
        }
        else
        {
            targetInSight = null;
        }
    }
}
