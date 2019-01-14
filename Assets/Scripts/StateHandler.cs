using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class StateRef
{
    public enum SpecialState
    {
        IDLE,
        RUN
    };
    public static int GetSpecialState(SpecialState state)
    {
        int numSpecial = 0;
        switch (state)
        {
            case SpecialState.IDLE:
                numSpecial = 0;
                break;
            case SpecialState.RUN:
                numSpecial = 1;
                break;
        }
        return numSpecial;
    }
}
public class StateHandler : MonoBehaviour
{
    [SerializeField]string currentAnimationState = string.Empty;
    private Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    void Update()
    {

    }
    public void MovingCharacter(Vector2 Axis)
    {

    }
    void UpdateState()
    {
        currentAnimationState = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
