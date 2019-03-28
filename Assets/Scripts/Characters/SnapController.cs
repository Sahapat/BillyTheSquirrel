using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{
    [SerializeField] BoxCollider snap_checker = null;
    [SerializeField] LayerMask CheckLayer= 0;
    void FixedUpdate()
    {
        if(GameCore.m_cameraController.isShake)
        {
            var hitInfo = PhysicsExtensions.OverlapBox(snap_checker,CheckLayer);
            if(hitInfo.Length >0)
            {
                LookAtTarget(hitInfo[0].transform.position);
            }
        }
    }
    void LookAtTarget(Vector3 position)
    {
        var lookAtPos = new Vector3(position.x,transform.position.y,position.z);
        transform.root.transform.LookAt(lookAtPos);
    }
}
