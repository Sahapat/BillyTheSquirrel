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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        m_stateHandler.MoveCharacter(movement);
    }
}
