using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore
{
    public static FloatingJoystick m_floatingJoystick;
    public static CameraController m_cameraController;
}
public class Main : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_IOS || UNITY_ANDROID
        GameCore.m_floatingJoystick = GameObject.Find("VirutalJoystick").GetComponentInChildren<FloatingJoystick>();
        #else
        GameCore.m_floatingJoystick = null;
        Destroy(GameObject.Find("VirutalJoystick"));
        #endif
        GameCore.m_cameraController = Camera.main.GetComponent<CameraController>();
    }
}
