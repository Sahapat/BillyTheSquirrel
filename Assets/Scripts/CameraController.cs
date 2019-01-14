using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform targetTranform = null;
    [Space]
    private Vector3 cameraOffset;
    [Header("Setting Property")]
    [SerializeField]float offset = 1f;
    [Range(0.01f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;
    [SerializeField] float rotateSpeedZ = 5.0f;
    [SerializeField] float rotateSpeedY = 5.0f;

    Vector2 m_TouchBeganPos = Vector2.zero;
    Vector3 angleAxisZ = Vector3.zero;
    Vector3 angleAxisY = Vector3.zero;
    void Awake()
    {
        angleAxisZ = Vector3.up;
        angleAxisY = Vector3.left;
    }
    void Start()
    {
        cameraOffset = new Vector3(offset,offset,offset) - targetTranform.position;
    }
    void LateUpdate()
    {
        Vector2 turnAxis = TurningInputGetter();
        Quaternion camTurnAngleZ = Quaternion.AngleAxis((turnAxis.x * rotateSpeedZ), angleAxisZ);
        Quaternion camTurnAngleY = Quaternion.AngleAxis((turnAxis.y * rotateSpeedY), angleAxisY);
        cameraOffset = camTurnAngleZ*camTurnAngleY * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(targetTranform);
    }
    Vector2 TurningInputGetter()
    {
        Vector2 Axis = Vector2.zero;
        #if UNITY_WSA_10_0
        if(Input.GetMouseButton(1))
        {
            Axis = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        }
        else
        {
            Axis = new Vector2(Input.GetAxis("Horizontal2"),Input.GetAxis("Vertical2"));
        }
        #else
        var m_touches = Input.touches;

        if(m_touches.Length >1)
        {
            if(m_touches[1].phase == TouchPhase.Began)
            {
                m_TouchBeganPos = m_touches[1].position;
            }
            else if(m_touches[1].phase == TouchPhase.Moved)
            {
                Vector2 direction = m_touches[1].position - m_TouchBeganPos;
                Axis = direction.normalized * 0.5f;
            }
        }
        #endif
        return Axis;
    }
}