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
    [SerializeField] float jumpInAirMoveScale = 15f;
    [Header("Drag")]
    [SerializeField] float DashDrag = 10f;
    [SerializeField] float normalDrag = 10f;
    [SerializeField] float fallDrag = 0;

    private StateHandler m_stateHandler = null;
    private Rigidbody m_rigidbody = null;
    private CapsuleCollider m_capsuleColider = null;
    private GroundChecker m_groundChecker = null;
    private CharacterState currentState = CharacterState.NONE;
    private Vector2 desire_Movement = Vector2.zero;
    private Vector2 real_Movement = Vector2.zero;
    private float CounterForJumpStart = 0f;
    private bool isCancelJump = false;

    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        m_stateHandler.OnStateChanged += OnStateChange;
    }
    void FixedUpdate()
    {
        desire_Movement = (m_stateHandler.Movement == Vector2.zero)?desire_Movement:m_stateHandler.Movement;
        real_Movement = m_stateHandler.Movement;

        m_rigidbody.drag = (m_groundChecker.isOnGround)?normalDrag:fallDrag;
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
                m_rigidbody.drag = DashDrag;
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
        var temp = desire_Movement.normalized;
        var Axis = new Vector3(desire_Movement.x, desire_Movement.y, 0);
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x, relativeVector.y);
        var degree = (radian * 180) / Mathf.PI;
        transform.eulerAngles = new Vector3(0, degree, 0);
    }
    void WhileJump()
    {
        var isHitTheWall = PhysicsExtensions.OverlapCapsule(m_capsuleColider,LayerMask.GetMask("Ground","Obtacle"));
        var addForceInAir = Vector2.ClampMagnitude(real_Movement,1f) * jumpInAirMoveScale;
        var currentVelocity = m_rigidbody.velocity;
        var increastFallValue = (4.2f * Time.deltaTime);

        if(isHitTheWall.Length > 0 || isCancelJump)
        {
            currentVelocity = new Vector3(0,currentVelocity.y-increastFallValue,0);
            isCancelJump = true;
        }
        else
        {
            currentVelocity = new Vector3(addForceInAir.x,currentVelocity.y-increastFallValue,addForceInAir.y);
        }
        m_rigidbody.velocity = currentVelocity;
    }
    void DoJump()
    {
        if (CounterForJumpStart <= Time.time)
        {
            var jumpMovement = real_Movement.normalized;
            jumpMovement = Vector2.ClampMagnitude(jumpMovement,1f) * 10f;
            var forceToAdd = new Vector3(jumpMovement.x,jumpUpScale,jumpMovement.y);
            m_rigidbody.AddForce(forceToAdd,ForceMode.Impulse);
            isCancelJump = false;
        }
    }
    void DoDash()
    {
        RotateToAxis();
        m_rigidbody.AddForce(transform.forward * dashScale,ForceMode.VelocityChange);
    }
    void DoMovement()
    {
        var desire_movement = Vector2.ClampMagnitude(desire_Movement,1f);
        var forceToMove = new Vector3(desire_movement.x,0f,desire_movement.y)*runSpeed;
        m_rigidbody.velocity = forceToMove;
    }
}
