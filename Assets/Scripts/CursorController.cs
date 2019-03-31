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
    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
