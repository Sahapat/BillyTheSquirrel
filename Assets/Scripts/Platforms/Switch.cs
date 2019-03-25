using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField,Tooltip("IActivable must derive in this Object")] GameObject[] targetObject = null;
    private IActivable[] targetIActivable = null;
    private Animator m_animator = null;
    private BoxCollider m_boxcolider = null;

    private bool _switchStatus = false;
    private bool switchStatus
    {
        get
        {
            return _switchStatus;
        }
        set
        {
            _switchStatus = value;
            OnChangeStatus();
        }
    }

    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_animator = GetComponent<Animator>();
    }
    void Start()
    {
        targetIActivable = new IActivable[targetObject.Length];
        for(int i =0;i<targetObject.Length;i++)
        {
            targetIActivable[i] = targetObject[i].GetComponent<IActivable>();
        }
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider,LayerMask.GetMask("Character"));
        if(hitInfo.Length > 0){OnPlayerEnter(hitInfo);}
    }
    void OnPlayerEnter(Collider[] enterPlayers)
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            switchStatus = !switchStatus;
        }
    }
    void OnChangeStatus()
    {
        m_animator.SetBool("isActive",switchStatus);
        if(switchStatus)
        {
            foreach(IActivable temp in targetIActivable)
            {
                temp?.Active();
            }
        }
        else
        {
            foreach(IActivable temp in targetIActivable)
            {
                temp?.InActive();
            }
        }
    }
}
