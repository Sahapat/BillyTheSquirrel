using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateHandler : MonoBehaviour
{
    [SerializeField] string currentAnimationState = string.Empty;
    private Animator m_animator;
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    void Update()
    {
        UpdateState();
    }
    public void MoveCharacter(Vector3 Axis)
    {
        float weight = new Vector2(Axis.x,Axis.y).magnitude;
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x, relativeVector.y);
        var degree = (radian * 180) / Mathf.PI;

        if (Axis != Vector3.zero)
        {
            transform.eulerAngles = new Vector3(0, degree, 0);
            if (weight > 0.5f)
            {
                m_animator.SetFloat("MovementSpeed", 2);
            }
            else
            {
                m_animator.SetFloat("MovementSpeed", 1);
            }
        }
        else
        {
            m_animator.SetFloat("MovementSpeed", 0);
        }
    }
    void UpdateState()
    {
        currentAnimationState = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
