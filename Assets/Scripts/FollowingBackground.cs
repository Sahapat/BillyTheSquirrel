using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingBackground : MonoBehaviour
{
    [SerializeField]Transform targetTranform = null;
    void LateUpdate()
    {
        if(!targetTranform)
        {
            targetTranform = GameCore.m_GameContrller.GetClientPlayerTarget().transform;
        }
    }
}
