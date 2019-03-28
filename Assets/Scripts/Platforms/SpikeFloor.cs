using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    [SerializeField] int damage = 30;
    [SerializeField] float delayBeforeActive = 1.2f;
    [SerializeField] float delayForInActive = 2f;


    private BoxCollider m_boxcolider = null;
    private Animator m_animator = null;

    private float counterForActive = 0f;
    private float counterForInActive = 0f;
    private bool hitTrigger = false;
    private HitDataStorage m_hitdataStorage = null;

    void Awake()
    {
        m_hitdataStorage = new HitDataStorage(10);
        m_boxcolider = GetComponent<BoxCollider>();
        m_animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Character", "Enemy"));

        if (hitInfo.Length > 0)
        {
            OnHitCharacter(hitInfo);
        }

        if (counterForActive <= Time.time && hitTrigger)
        {
            m_animator.SetBool("isActive", true);
            counterForInActive = Time.time + delayForInActive;
            hitTrigger = false;
        }
        else if(m_animator.GetBool("isActive") && counterForInActive <= Time.time)
        {
            m_animator.SetBool("isActive",false);
            ResetVar();
        }
    }
    void OnHitCharacter(Collider[] hits)
    {
        var isActive = m_animator.GetBool("isActive");

        if (isActive)
        {
            foreach(var temp in hits)
            {
                if(m_hitdataStorage.CheckHit(temp.GetInstanceID()))
                {
                    var attackableObj = temp.GetComponent<IAttackable>();
                    attackableObj.TakeDamage(damage);
                }
            }
        }
        else
        {
            if (!hitTrigger)
            {
                counterForActive = Time.time + delayBeforeActive;
                hitTrigger = true;
            }
        }
    }
    void ResetVar()
    {
        hitTrigger = false;
        m_hitdataStorage.ResetHit();
    }
}
