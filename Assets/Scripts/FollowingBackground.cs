using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingBackground : MonoBehaviour
{
    Transform targetTranform = null;

    Vector3 offsetToTarget = Vector3.zero;
    void Start()
    {
        targetTranform = GameCore.m_GameContrller.GetClientPlayerTarget().transform;
        offsetToTarget = transform.position - targetTranform.position;
    }
    void LateUpdate()
    {
        if(!targetTranform)return;
        transform.position = targetTranform.position + offsetToTarget;
    }
}
