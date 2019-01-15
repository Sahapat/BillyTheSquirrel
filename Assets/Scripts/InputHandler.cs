using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private StateHandler m_stateHandler;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (GameCore.m_floatingJoystick)
        {
            movement.x = GameCore.m_floatingJoystick.Horizontal;
            movement.y = GameCore.m_floatingJoystick.Vertical;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        m_stateHandler.MoveCharacter(movement);
    }
}
