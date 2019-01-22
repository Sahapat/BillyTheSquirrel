using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore
{
    public static FloatingJoystick m_floatingJoystick;
    public static CameraController m_cameraController;
    public static UIHandler m_uiHandler;
    public static Main m_Main;
}
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
        #if UNITY_IOS || UNITY_ANDROID
        GameCore.m_floatingJoystick = GameObject.Find("VirutalJoystick").GetComponentInChildren<FloatingJoystick>();
        #else
        GameCore.m_floatingJoystick = null;
        Destroy(GameObject.Find("VirutalJoystick"));
        #endif
        GameCore.m_cameraController = Camera.main.GetComponent<CameraController>();
        GameCore.m_uiHandler = GetComponent<UIHandler>();
        GameCore.m_Main = GetComponent<Main>();
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

