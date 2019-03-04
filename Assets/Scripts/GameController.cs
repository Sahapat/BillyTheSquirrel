using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Player ClientPlayerTarget = null;
    public Player GetClientPlayerTarget()
    {
        return ClientPlayerTarget;
    }
    public void SwitchAvtiveInventory()
    {
        var inventoryStatus = GameCore.m_uiHandler.GetInventoryStatus();
        if(inventoryStatus)
        {
            GameCore.m_uiHandler.CloseInventory();
            GameCore.m_cameraController.SetCameraState(CameraController.CameraState.NORMAL);
            GameCore.m_CursorController.SetCursorInGameMode();
        }
        else
        {
            GameCore.m_uiHandler.OpenInventory();
            GameCore.m_cameraController.SetCameraState(CameraController.CameraState.INVENTORY);
            GameCore.m_CursorController.SetCursorInInventoryMode();
        }
    }
}
