using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector3 movement = Vector3.zero;
    private StateHandler m_stateHandler = null;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
    }
    void Update()
    {
        MovementInputGetter();
    }
    void FixedUpdate()
    {
        m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));
        if (NormalAttackGetter() && GameCore.m_GameContrller.GetClientPlayerTarget().CheckNormalAttackSP())
        {
            if(m_stateHandler.NormalAttack())
            {
                GameCore.m_GameContrller.GetClientPlayerTarget().NormalAttackDepletion();
            }
        }
        if (HeavyAttackGetter() && GameCore.m_GameContrller.GetClientPlayerTarget().CheckHeavyAttackSP())
        {
            if(m_stateHandler.HeavyAttack())
            {
                GameCore.m_GameContrller.GetClientPlayerTarget().HeavyAttackDepletion();
            }
        }
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button1))&&GameCore.m_GameContrller.GetClientPlayerTarget().CheckDashSP())
        {
           if( m_stateHandler.Dash())
           {
               GameCore.m_GameContrller.GetClientPlayerTarget().DashDepletion();
           }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            m_stateHandler.Jump();
        }
    }
    void MovementInputGetter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    bool NormalAttackGetter()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetMouseButtonDown(0);
    }
    bool HeavyAttackGetter()
    {
        var TriggerAxis = Input.GetAxis("JoystickTrigger");
        return TriggerAxis > 0 || Input.GetMouseButtonDown(1);
    }
    Vector3 SerializeInputByCameraTranform(Vector3 inputAxis)
    {
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized * inputAxis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized * inputAxis.x;
        Vector3 direction = newForward + newRight;
        return new Vector3(direction.x, direction.z);
    }
}
