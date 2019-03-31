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
    [SerializeField] float offsetX = 3f;
    [SerializeField] float offsetY = 0.8f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;
    [SerializeField] float xSpeed = 30f;
    [SerializeField] float ySpeed = 45f;
    [SerializeField] float yMinLimit = -180;
    [SerializeField] float yMaxLimit = 180;
    
    public bool isShake {get;private set;} = false;

    CameraState m_cameraState = CameraState.NORMAL;
    private float RotateX;
    private float RotateY;
    private float CounterForShakeDelay = 0f;
    private float CounterForShakeDuration = 0f;
    private float shakeLenght = 0f;
    void Start()
    {
        targetTranform = GameCore.m_GameContrller.GetClientPlayerTarget().transform;
        RotateY = 25f;
    }
    void LateUpdate()
    {
        if (targetTranform == null) return;

        if (CounterForShakeDelay >= Time.time || !isShake)
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
    }
    void InventoryCamera()
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

        transform.rotation = rotation;
        transform.position = Vector3.Lerp(transform.position, position, smoothFactor);
    }
    void TurningInputGetter()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            RotateX += Input.GetAxis("Mouse X") * xSpeed * offsetX * Time.fixedDeltaTime;
            RotateY += Input.GetAxis("Mouse Y") * ySpeed * Time.fixedDeltaTime;
        }
        else
        {
            RotateX += Input.GetAxis("Horizontal2") * xSpeed * offsetX * Time.deltaTime * 1.5f;
            RotateY += Input.GetAxis("Vertical2") * ySpeed * Time.deltaTime * 1.5f;
        }
        RotateX = ClampAngle(RotateX);
        RotateY = ClampAngle(RotateY);
        RotateY = Mathf.Clamp(RotateY,yMinLimit,yMaxLimit);
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
    public void SetCameraState(CameraState state)
    {
        m_cameraState = state;
    }
    public void SetCameraTarget(Transform targetTranform)
    {
        this.targetTranform = targetTranform;
    }
    public void ShakeCamera(float time,float shakeLenght)
    {
        iTween.ShakePosition(this.gameObject,new Vector3(shakeLenght,shakeLenght,0),time);
    }
}