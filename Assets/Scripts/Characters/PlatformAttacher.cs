using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformAttacher : MonoBehaviour
{
    [SerializeField]LayerMask platformMask = 0;
    private BoxCollider m_boxcolider = null;
    private Transform rootTranform = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        rootTranform = transform.root;
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider,platformMask);

        if(hitInfo.Length > 0)
        {
            rootTranform.parent = hitInfo[0].transform;
        }
        else
        {
            rootTranform.parent = null;
        }
    }
}
