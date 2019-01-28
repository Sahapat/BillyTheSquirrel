using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore
{
    public static CameraController m_cameraController;
    public static UIHandler m_uiHandler;
    public static Main m_Main;
    public static InventoryUIController m_inventoryController;
}
public enum SceneInfo
{
    MAINMENU,
    GAME    
};
public class Main : MonoBehaviour
{
    int _coin = 0;
    public int Coin
    {
        get
        {
            return 0;   
        }
        set
        {
            _coin += value;
            _coin =Mathf.Clamp(_coin,0,9999);
            GameCore.m_uiHandler.UpdateTxtCoin(_coin);
        }
    }

    public bool isJoy
    {
        get;private set;
    }
    void Awake()
    {
        GameCore.m_cameraController = Camera.main.GetComponent<CameraController>();
        GameCore.m_uiHandler = GetComponent<UIHandler>();
        GameCore.m_Main = GetComponent<Main>();
        GameCore.m_inventoryController = FindObjectOfType<InventoryUIController>();
    }
    void Start()
    {
        SettingValue();
    }
    void SettingValue()
    {
        Coin = 0;
    }
}

