using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private StateHandler m_stateHandler = null;
    private GroundChecker m_groundChecker = null;
    private Vector3 movement = Vector3.zero;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
    }
    void Update()
    {
        if (m_groundChecker.isOnGround)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            GameCore.m_uiHandler.SwitchActiveInventoryHUB();
        }
    }
    void FixedUpdate()
    {
        m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));
    }
    Vector3 SerializeInputByCameraTranform(Vector3 inputAxis)
    {
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized * inputAxis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized * inputAxis.x;
        Vector3 direction = newForward + newRight;
        return new Vector3(direction.x, direction.z);
    }
}
