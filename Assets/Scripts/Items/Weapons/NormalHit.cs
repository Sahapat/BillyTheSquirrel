using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHit : MonoBehaviour, IHitSystem
{
    [SerializeField] int damagePerHit = 0;
    [SerializeField] float m_delayForActive = 0f;
    [SerializeField] float m_delayForInActive = 0f;
    [SerializeField] LayerMask TargetLayer = 0;
    public float delayForActive
    {
        get
        {
            return m_delayForActive;
        }
        private set
        {
            m_delayForActive = value;
            m_delayForActive = Mathf.Clamp(m_delayForActive, 0f, float.MaxValue);
        }
    }

    public float delayForInActive
    {
        get
        {
            return m_delayForInActive;
        }
        private set
        {
            m_delayForInActive = value;
            m_delayForInActive = Mathf.Clamp(m_delayForInActive, 0f, float.MaxValue);
        }
    }
    private bool isActive = false;
    private bool isSetActive = false;
    private float ActiveDelayCounter = 0f;
    private float InActionDelayCounter = 0f;
    bool isHit = false;
    private BoxCollider m_boxcolider = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        TimeCountChecker();
        if (isActive)
        {
            var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, TargetLayer);
            if (hitInfo.Length > 0 && !isHit)
            {
                hitInfo[0].GetComponent<ICharacter>().TakeDamage(damagePerHit);
                isHit = true;
            }
        }
    }
    public void ActiveHit()
    {
        isSetActive = true;
        ActiveDelayCounter = Time.time + delayForActive;
        InActionDelayCounter = ActiveDelayCounter + delayForInActive;
    }

    public void CancelHit()
    {
        ResetHit();
    }
    void TimeCountChecker()
    {
        if (ActiveDelayCounter <= Time.time && isSetActive)
        {
            isActive = true;
            isSetActive = false;
        }
        if (InActionDelayCounter <= Time.time && isActive)
        {
            ResetHit();
        }
    }
    void ResetHit()
    {
        ClearHitObj();
        isActive = false;
        isSetActive = false;
    }
    void ClearHitObj()
    {
        isHit = false;
    }
}
