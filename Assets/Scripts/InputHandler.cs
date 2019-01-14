using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private StateHandler m_stateHandler;
    private FloatingJoystick m_floatingJoystick;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_floatingJoystick = FindObjectOfType<FloatingJoystick>();
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;
        #if UNITY_WSA_10_0
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        #else
        movement.x = m_floatingJoystick.Horizontal;
        movement.y = m_floatingJoystick.Vertical;
        #endif
        m_stateHandler.MoveCharacter(movement);
    }
}
