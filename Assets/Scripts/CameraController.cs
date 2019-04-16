using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraState
    {
        NORMAL,
        UI_MANAGE,
        LOCK_ON,
        NONE
    };

    [SerializeField] Transform targetTranform = null;
    [Space]
    [Header("Setting Property")]
    [SerializeField] float offsetX = 3f;
    [SerializeField] float offsetY = 0.8f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;
    [SerializeField] float xSpeed = 30f;
    [SerializeField] float ySpeed = 45f;
    [SerializeField] float yMinLimit = -180;
    [SerializeField] float yMaxLimit = 180;

    public bool isShake { get; private set; } = false;

    CameraState m_cameraState = CameraState.NORMAL;
    private float RotateX;
    private float RotateY;
    void Start()
    {
        targetTranform = GameCore.m_GameContrller.ClientPlayerTarget.transform;
        RotateY = 25f;
    }
    void LateUpdate()
    {
        //Checking Target if it's null turn camera state to NONE
        if (!targetTranform)
        {
            m_cameraState = CameraController.CameraState.NONE;
        }
        //switch for doing a state
        switch (m_cameraState)
        {
            case CameraState.NORMAL:
                ThirdPersonCamera();
                break;
            case CameraState.UI_MANAGE:
                Ui_ManageCamera();
                break;
            case CameraState.LOCK_ON:
                Lock_OnCamera();
                break;
        }
    }

    void Lock_OnCamera()
    {
        Quaternion rotation = Quaternion.Euler(RotateY, RotateX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -offsetX);
        Vector3 position = rotation * negDistance + (targetTranform.position + (Vector3.up * offsetY));

        transform.rotation = rotation;
        transform.position = position;
    }

    void Ui_ManageCamera()
    {
        Quaternion rotation = Quaternion.Euler(RotateY, RotateX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -offsetX);
        Vector3 position = rotation * negDistance + (targetTranform.position + (Vector3.up * offsetY));
        transform.rotation = rotation;
        transform.position = position;
    }
    void ThirdPersonCamera()
    {
        TurningInputGetter();
        Quaternion rotation = Quaternion.Euler(RotateY, RotateX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -offsetX);
        Vector3 position = rotation * negDistance + (targetTranform.position + (Vector3.up * offsetY));

        transform.rotation = Quaternion.Lerp(transform.rotation,rotation,smoothFactor);
        transform.position = Vector3.Lerp(transform.position, position, smoothFactor);
    }
    void TurningInputGetter()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            RotateX += Input.GetAxis("Mouse X") * xSpeed * offsetX * Time.fixedDeltaTime;
            RotateY -= Input.GetAxis("Mouse Y") * ySpeed * Time.fixedDeltaTime;
        }
        else
        {
            RotateX += Input.GetAxis("Horizontal2") * xSpeed * offsetX * Time.deltaTime * 1.5f;
            RotateY += Input.GetAxis("Vertical2") * ySpeed * Time.deltaTime * 1.5f;
        }

        RotateX = ClampAngle(RotateX);
        RotateY = ClampAngle(RotateY);

        RotateY = Mathf.Clamp(RotateY, yMinLimit, yMaxLimit);
    }
    float ClampAngle(float angle)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return angle;
    }
    public void SetCameraNormalState()
    {
        m_cameraState = CameraController.CameraState.NORMAL;
    }
    public void SetCameraUI_ManageState()
    {
        m_cameraState = CameraController.CameraState.UI_MANAGE;
    }
    public void SetCameraLock_OnState()
    {
        m_cameraState = CameraController.CameraState.LOCK_ON;
    }
    public void ShakeCamera(float time, float shakeLenght)
    {
        iTween.ShakePosition(this.gameObject, new Vector3(shakeLenght, shakeLenght, 0), time);
    }
}