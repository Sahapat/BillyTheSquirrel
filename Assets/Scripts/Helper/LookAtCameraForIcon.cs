using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraForIcon : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
