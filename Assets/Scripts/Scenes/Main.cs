using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore
{
    public static CameraController m_cameraController;
    public static UIHandler m_uiHandler;
    public static Main m_Main;
    public static InventoryUIController m_inventoryController;
    public static GameController m_GameContrller;
    public static CursorController m_CursorController;
}
public enum SceneInfo
{
    MAINMENU,
    GAME    
};
public class Main : MonoBehaviour
{
    void Awake()
    {
        GameCore.m_cameraController = Camera.main.GetComponent<CameraController>();
        GameCore.m_uiHandler = FindObjectOfType<UIHandler>();
        GameCore.m_Main = GetComponent<Main>();
        GameCore.m_inventoryController = FindObjectOfType<InventoryUIController>();
        GameCore.m_GameContrller = GetComponent<GameController>();
        GameCore.m_CursorController = GetComponent<CursorController>();
    }
    void Start()
    {
        GameCore.m_uiHandler.CloseInventory();
        GameCore.m_CursorController.SetCursorInGameMode();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            FindObjectOfType<FadeController>().LoadSceneAndFade(2);
        }
    }
}

