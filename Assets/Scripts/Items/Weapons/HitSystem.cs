using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : MonoBehaviour, IHitSystem
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
    private HitDataStorage m_hitDataStorage = null;
    private BoxCollider m_boxcolider = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_hitDataStorage = new HitDataStorage(8);
    }
    void FixedUpdate()
    {
        if (!isSetActive) return;
        Counting();
        if (isActive)
        {
            CheckHit();
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
    void ResetHit()
    {
        isActive = false;
        isSetActive = false;
        m_hitDataStorage.ResetHit();
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, TargetLayer);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var character = hitInfo[i].GetComponent<ICharacter>();
                character.TakeDamage(damagePerHit);
            }
        }
    }
    void Counting()
    {
        if (ActiveDelayCounter >= Time.time)
        {
            isActive = true;
        }
        if (InActionDelayCounter >= Time.time)
        {
            ResetHit();
        }
    }
}
