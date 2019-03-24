using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Player ClientPlayerTarget = null;
    [SerializeField] Transform temporaryTranform = null;

    private float CounterForPause = 0f;

    public Player GetClientPlayerTarget()
    {
        return ClientPlayerTarget;
    }
    public Transform GetTemporaryTranform()
    {
        return temporaryTranform;
    }
    void Update()
    {
        if (Time.timeScale == 0)
        {
            if(CounterForPause <= Time.unscaledTime)
            {
                Time.timeScale = 1;
            }
        }
    }
    public void SwitchAvtiveInventory()
    {
        var inventoryStatus = GameCore.m_uiHandler.GetInventoryStatus();
        if (inventoryStatus)
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
    public void UpdateEquipmentSlot()
    {
        GameCore.m_inventoryController.UpdateShield();
        GameCore.m_inventoryController.UpdateWeapon();
    }
}
