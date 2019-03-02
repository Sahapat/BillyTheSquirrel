using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateHandler))]
public class MovementHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 0f;
    [SerializeField] float shieldRunSpeed = 0f;
    [Header("Dash")]
    [SerializeField] float dashScale = 20f;
    [Header("Jump")]
    [SerializeField] float jumpStartDelay = 0.2f;
    [SerializeField] float jumpUpScale = 3.2f;
    [SerializeField] float jumpMoveScale = 2.5f;
    [SerializeField] float jumpInAirMoveScale = 15f;
    [SerializeField] float jumpMaxMagnitude = 3.5f;
    [Header("Drag")]
    [SerializeField] float normalDrag = 10f;
    [SerializeField] float fallDrag = 0;

    private StateHandler m_stateHandler = null;
    private Rigidbody m_rigidbody = null;
    private GroundChecker m_groundChecker = null;
    private CharacterState currentState = CharacterState.NONE;
    private Vector2 Movement = Vector2.zero;
    private float CounterForJumpStart = 0f;

    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
    }
    void Start()
    {
        m_stateHandler.OnStateChanged += OnStateChange;
    }
    void FixedUpdate()
    {
        Movement = m_stateHandler.Movement;
        m_rigidbody.drag = (m_groundChecker.isOnGround) ? normalDrag : fallDrag;
        switch (currentState)
        {
            case CharacterState.RUN:
                RotateToAxis();
                DoMovement();
                break;
            case CharacterState.JUMP:
                DoJump();
                break;
            case CharacterState.JUMPINAIR:
                RotateToAxis();
                WhileJump();
                break;
            case CharacterState.DASH:
                m_rigidbody.drag = normalDrag;
                break;
        }
    }
    void OnStateChange()
    {
        currentState = m_stateHandler.currentCharacterState;
        switch(currentState)
        {
            case CharacterState.JUMP:
                CounterForJumpStart = Time.time + jumpStartDelay;
            break;
            case CharacterState.DASH:
                DoDash();
            break;
        }
    }
    void RotateToAxis()
    {
        var temp = Movement.normalized;
        var Axis = new Vector3(Movement.x, Movement.y, 0);
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x, relativeVector.y);
        var degree = (radian * 180) / Mathf.PI;
        transform.eulerAngles = new Vector3(0, degree, 0);
    }
    void WhileJump()
    {
        m_rigidbody.AddForce(new Vector3(Movement.x * jumpMoveScale * jumpInAirMoveScale, 0, Movement.y * jumpMoveScale * jumpInAirMoveScale));
        var currentMagnitude = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.z).magnitude;
        if (currentMagnitude > jumpMaxMagnitude)
        {
            var ClampMagnitude = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.z).normalized * jumpMaxMagnitude;
            m_rigidbody.velocity = new Vector3(ClampMagnitude.x, m_rigidbody.velocity.y, ClampMagnitude.y);
        }
    }
    void DoJump()
    {
        if (CounterForJumpStart <= Time.time)
        {
            var jumpMovement = Movement.normalized;
            m_rigidbody.velocity = new Vector3(jumpMovement.x * jumpMoveScale, jumpUpScale, jumpMovement.y * jumpMoveScale);
        }
    }
    void DoDash()
    {
        RotateToAxis();
        m_rigidbody.AddForce(transform.forward * dashScale,ForceMode.VelocityChange);
    }
    void DoMovement()
    {
        m_rigidbody.velocity = new Vector3(Movement.x * runSpeed, m_rigidbody.velocity.y, Movement.y * runSpeed);
    }
}
