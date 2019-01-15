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
        #if UNITY_WSA_10_0
        GameCore.m_floatingJoystick = null;
        Destroy(GameObject.Find("VirutalJoystick"));
        #else
        GameCore.m_floatingJoystick = GameObject.Find("VirutalJoystick").GetComponentInChildren<FloatingJoystick>();
        #endif

        GameCore.m_cameraController = Camera.main.GetComponent<CameraController>();
    }
}
