using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]CursorLockMode cursorLockMode = CursorLockMode.None;
    [SerializeField]bool cursor_visable = false;

    void Start()
    {
        UpdateCursor();
    }
    public void SetCursor(bool status,CursorLockMode lockMode)
    {
        cursor_visable = status;
        cursorLockMode = lockMode;
        UpdateCursor();
    }
    void UpdateCursor()
    {
        Cursor.lockState = cursorLockMode;
        Cursor.visible = cursor_visable;
    }
}
