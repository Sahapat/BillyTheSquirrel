using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateHandler))]
public class ActionHandler : MonoBehaviour
{
    private StateHandler m_stateHandler = null;
    private BaseSword m_baseSword = null;
    
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_baseSword = GetComponentInChildren<BaseSword>();
    }
    void Start()
    {
        m_stateHandler.OnStateChanged += OnActionState;
    }
    void OnActionState()
    {
        switch(m_stateHandler.currentCharacterState)
        {
            case CharacterState.ATTACK1:
                m_baseSword.hitSystemManager.ActiveNormalHit(0);
            break;
            case CharacterState.ATTACK2:
                m_baseSword.hitSystemManager.ActiveNormalHit(1);
            break;
            case CharacterState.ATTACK3:
                m_baseSword.hitSystemManager.ActiveNormalHit(2);
            break;
            case CharacterState.ATTACKHEAVY:
                m_baseSword.hitSystemManager.ActiveHeavyHit();
            break;
            case CharacterState.RESET:
                m_baseSword.hitSystemManager.CancelAllHit();
            break;
        }
    }
    void UpdateSword(BaseSword baseSword)
    {
        m_baseSword = baseSword;
    }
}
