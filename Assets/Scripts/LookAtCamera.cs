using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform m_Camera = null;
    void Awake()
    {
        m_Camera = Camera.main.transform;
    }
    void FixedUpdate()
    {
        transform.LookAt(m_Camera);
    }
}
