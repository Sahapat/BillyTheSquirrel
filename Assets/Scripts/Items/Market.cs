using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] GameObject marketUI = null;
    private BoxCollider m_boxcolider = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Character"));

        if (hitInfo.Length > 0)
        {
            OnPlayerEnter(hitInfo[0]);
        }
    }
    void OnPlayerEnter(Collider enter)
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) && !marketUI.activeSelf)
        {
            marketUI.SetActive(true);
            GameCore.m_CursorController.EnableCursor();
            GameCore.m_cameraController.SetCameraUI_ManageState();
            GameCore.m_GameContrller.Controlable = false;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) && marketUI.activeSelf)
        {
            marketUI.SetActive(false);
            GameCore.m_CursorController.DisableCursor();
            GameCore.m_cameraController.SetCameraNormalState();
            GameCore.m_GameContrller.Controlable = true;
        }
    }
}
