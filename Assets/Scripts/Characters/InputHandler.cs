using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Player m_player = null;
    private Vector3 movement = Vector3.zero;
    private StateHandler m_stateHandler = null;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_player = GetComponent<Player>();
    }
    void Update()
    {
        MovementInputGetter();
    }
    void FixedUpdate()
    {
        m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));
    }
    void MovementInputGetter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    Vector3 SerializeInputByCameraTranform(Vector3 inputAxis)
    {
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized * inputAxis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized * inputAxis.x;
        Vector3 direction = newForward + newRight;
        return new Vector3(direction.x, direction.z);
    }
}
