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

    void Start()
    {
        cameraOffset = transform.position - targetTranform.position;
    }
    void LateUpdate()
    {
        Quaternion camTurnAngleZ = Quaternion.AngleAxis((Input.GetAxis("Mouse X") * rotateSpeedZ), Vector3.up);
        Quaternion camTurnAngleY = Quaternion.AngleAxis((Input.GetAxis("Mouse Y") * rotateSpeedY), Vector3.left);
        cameraOffset = camTurnAngleZ*camTurnAngleY * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(targetTranform);
    }
}
