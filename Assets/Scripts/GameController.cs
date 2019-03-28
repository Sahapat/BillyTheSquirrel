using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum CoutrollerInUse
    {   
        XBOX,
        KEYBOARD
    };

    [SerializeField] Player ClientPlayerTarget = null;
    [SerializeField] Transform temporaryTranform = null;
    [SerializeField] float ClampPlayerByYPosition = 0f;


    void FixedUpdate()
    {
        if(ClientPlayerTarget.isDead)
        {
            GameCore.m_uiHandler.ShowGameOver();
        }
        else
        {
            GameCore.m_uiHandler.CloseGameOver();
        }
        if(ClientPlayerTarget.transform.position.y < ClampPlayerByYPosition)
        {
            var spawnPos = ClientPlayerTarget.CurrentGround.position;
            spawnPos = new Vector3(spawnPos.x,spawnPos.y + 10,spawnPos.z);
            ClientPlayerTarget.transform.position = spawnPos;
        }
    }
    public Player GetClientPlayerTarget()
    {
        return ClientPlayerTarget;
    }
    public Transform GetTemporaryTranform()
    {
        return temporaryTranform;
    }
    public void SwitchActiveInventory()
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
