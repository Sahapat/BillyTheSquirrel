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
    [Header("Setting Property")]
    [SerializeField] float offsetZ = 1f;
    [SerializeField] float offsetY = 2f;
    [Range(0.5f, 1.5f)]
    [SerializeField] float smoothFactor = 0.5f;
    [SerializeField] float rotateSpeedZ = 5.0f;
    [SerializeField] float rotateSpeedY = 5.0f;

    Vector2 m_TouchBeganPos = Vector2.zero;
    Vector3 angleAxisZ = Vector3.zero;
    Vector3 angleAxisY = Vector3.zero;
    Vector3 cameraOffset;
    CameraState m_cameraState = CameraState.NORMAL;
    void Awake()
    {
        targetTranform = (targetTranform) ? targetTranform : GameObject.FindGameObjectWithTag("Player").transform;
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
        switch (m_cameraState)
        {
            case CameraState.NORMAL:
                ThirdPersonCamera();
                break;
            case CameraState.INVENTORY:
                InventoryCamera();
                break;
        }
    }
    void InventoryCamera()
    {
        RotateCameraAxisZ(Vector2.zero);
        RotateCameraAxisY(Vector2.zero);
        transform.LookAt(targetTranform);
        transform.Translate(new Vector3(0, offsetY, 0));
    }
    void ThirdPersonCamera()
    {
        RotateCameraAxisZ(TurningInputGetter());
        RotateCameraAxisY(TurningInputGetter());
        transform.LookAt(targetTranform);
        transform.Translate(new Vector3(0, offsetY, 0));
    }
    void RotateCameraAxisZ(Vector2 turnAxis)
    {
        Quaternion camTurnAngleZ = Quaternion.AngleAxis((turnAxis.x * rotateSpeedZ), angleAxisZ);
        cameraOffset = camTurnAngleZ * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }
    void RotateCameraAxisY(Vector2 turnAxis)
    {
        Quaternion camTurnAngleY = Quaternion.AngleAxis((turnAxis.y * rotateSpeedY), angleAxisY);
        cameraOffset = camTurnAngleY * cameraOffset;
        Vector3 newPos = targetTranform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }
    Vector2 TurningInputGetter()
    {
        float rotateAxisX = 0f;
        float rotateAxisY = 0f;
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            rotateAxisX = Input.GetAxis("Mouse X");
            rotateAxisY = Input.GetAxis("Mouse Y");
        }
        else
        {
            rotateAxisX = Input.GetAxis("Horizontal2");
            rotateAxisY = Input.GetAxis("Vertical2");
            rotateAxisX *= 1.5f;
            rotateAxisY *= 2.2f;
        }
        return new Vector2(rotateAxisX, rotateAxisY);
    }
    public void SetCameraState(CameraState state)
    {
        m_cameraState = state;
    }
    public void SetCameraTarget(Transform targetTranform)
    {
        this.targetTranform = targetTranform;
    }
}