using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("InGameMode")]
    [SerializeField]CursorLockMode gameMode_cursorLockMode = CursorLockMode.None;
    [SerializeField]bool gameMode_cursor_visable = false;
    [Header("UIMode")]
    [SerializeField]CursorLockMode inventoryMode_cursorLockMode = CursorLockMode.None;
    [SerializeField]bool uiMode_cursor_visable = false;
    public void SetCursorInGameMode()
    {
        Cursor.visible =gameMode_cursor_visable;
        Cursor.lockState = gameMode_cursorLockMode;
        GameCore.m_cameraController.SetCameraState(CameraController.CameraState.NORMAL);
    }
    public void SetCursorInInventoryMode()
    {
        Cursor.visible = uiMode_cursor_visable;
        Cursor.lockState = inventoryMode_cursorLockMode;
        GameCore.m_cameraController.SetCameraState(CameraController.CameraState.INVENTORY);
    }
}
