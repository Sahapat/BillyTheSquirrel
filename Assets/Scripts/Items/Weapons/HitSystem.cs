using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : BaseHitSystem
{
    [SerializeField] TrailRenderer m_trailRenderer = null;
    [SerializeField] float forceToAdd = 5f;
    [SerializeField] float shakeLeght = 0.32f;
    private BoxCollider m_boxcolider = null;
    private Transform m_rootTranform = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_hitDataStorage = new HitDataStorage(5);
        m_rootTranform = transform.root;
        m_trailRenderer.enabled = false;
    }
    void FixedUpdate()
    {
        if (isActive)
        {
            CheckHit();
            isActive = (activeDurationCounter >= Time.time);
            m_trailRenderer.enabled = true;
            if (!isActive)
            {
                ResetHit();
            }
        }
    }
    public override void ActiveHit()
    {
        isActive = true;
        activeDurationCounter = Time.time + activeDuration;
    }
    public override void CancelHit()
    {
        ResetHit();
    }
    void ResetHit()
    {
        m_trailRenderer.enabled = false;
        isActive = false;
        m_hitDataStorage.ResetHit();
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, TargetLayer);
        if (hitInfo.Length == 0) return;
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var attackableObj = hitInfo[i].GetComponent<IAttackable>();
                if (hitInfo[i].CompareTag("Player") || hitInfo[i].CompareTag("Enemy"))
                {
                    attackableObj.TakeDamage(damagePerHit, m_rootTranform.forward * forceToAdd);
                }
                else
                {
                    attackableObj.TakeDamage(damagePerHit);
                }
                if(shakeLeght != 0)
                {
                    GameCore.m_cameraController.ShakeCamera(0.18f,shakeLeght);
                }
            }
        }
    }
}
