using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraState
    {
        NORMAL,
        INVENTORY
    };

    [SerializeField] Transform targetTranform = null;
    [Space]
    private Vector3 cameraOffset;
    [Header("Setting Property")]
    [SerializeField] float offsetZ = 1f;
    [SerializeField] float offsetY = 2f;
    [Range(0.01f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;
    [SerializeField] float rotateSpeedZ = 5.0f;
    [SerializeField] float rotateSpeedY = 5.0f;

    Vector2 m_TouchBeganPos = Vector2.zero;
    Vector3 angleAxisZ = Vector3.zero;
    Vector3 angleAxisY = Vector3.zero;
    
    CameraState m_cameraState = CameraState.NORMAL;

    void Awake()
    {
        angleAxisZ = Vector3.up;
        angleAxisY = Vector3.left;
        Cursor.visible = false;
    }
    void Start()
    {
        cameraOffset = new Vector3(targetTranform.position.x, offsetZ, targetTranform.position.z - offsetZ) - targetTranform.position;
        ThirdPersonCamera();
        Quaternion camTurnAngleY = Quaternion.AngleAxis((9f * rotateSpeedY), angleAxisY);
        cameraOffset = camTurnAngleY * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(targetTranform);
    }
    void LateUpdate()
    {
        ThirdPersonCamera();
    }
    void ThirdPersonCamera()
    {
        Vector2 turnAxis = TurningInputGetter();
        Quaternion camTurnAngleZ = Quaternion.AngleAxis((turnAxis.x * rotateSpeedZ), angleAxisZ);
        Quaternion camTurnAngleY = Quaternion.AngleAxis((turnAxis.y * rotateSpeedY), angleAxisY);
        cameraOffset = camTurnAngleZ * camTurnAngleY * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(targetTranform);
        transform.Translate(new Vector3(0, offsetY, 0));
    }
    Vector2 TurningInputGetter()
    {
        Vector2 Axis = Vector2.zero;
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Axis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        else
        {
            Axis = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
            Axis *= 1.5f;
        }
        return Axis;
    }
    public void SetCameraState(CameraState state)
    {
        m_cameraState = state;
    }
}