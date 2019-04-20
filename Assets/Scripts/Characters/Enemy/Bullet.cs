using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]float speed = 5f;
    [SerializeField]SphereCollider m_sphereColider = null;
    void FixedUpdate()
    {
        transform.position += transform.forward * (speed*Time.deltaTime);
        var hitInfo = PhysicsExtensions.OverlapSphere(m_sphereColider,LayerMask.GetMask("Ground","Obtacle"));
        if(hitInfo.Length > 0)
        {
            Destroy(this.gameObject);
        }
    }
}
