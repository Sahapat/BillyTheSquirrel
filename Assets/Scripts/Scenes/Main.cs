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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            GameCore.m_uiHandler.CloseMarket();
            GameCore.m_uiHandler.CloseInventory();
            GameCore.m_GameContrller.Controlable = true;
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.AddCoin(50);
        }
        if(Input.GetKeyDown(KeyCode.F7))
        {
            GameCore.m_GameContrller.ClientPlayerTarget.transform.position = new Vector3(85.33f,18.82f,106.66f);
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            LoadGameScene("Level2");
        }
    }
    public void LoadGameScene(string LevelPreset)
    {
        FindObjectOfType<FadeController>().LoadSceneAndFade(LevelLoadHelper.GetRandomLevel(LevelPreset));
    }
}

