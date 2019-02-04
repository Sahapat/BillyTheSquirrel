using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("InGameMode")]
    [SerializeField]CursorLockMode gameMode_cursorLockMode = CursorLockMode.None;
    [SerializeField]bool gameMode_cursor_visable = false;
    [Header("UIMode")]
    [SerializeField]CursorLockMode uiMode_cursorLockMode = CursorLockMode.None;
    [SerializeField]bool uiMode_cursor_visable = false;
    public void CursorInGameMode()
    {
        Cursor.visible =gameMode_cursor_visable;
        Cursor.lockState = gameMode_cursorLockMode;
        GameCore.m_cameraController.SetCameraState(CameraController.CameraState.NORMAL);
    }
    public void CursorInUiMode()
    {
        Cursor.visible = uiMode_cursor_visable;
        Cursor.lockState = uiMode_cursorLockMode;
        GameCore.m_cameraController.SetCameraState(CameraController.CameraState.INVENTORY);
    }
}
