using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : BaseHitSystem
{
    [SerializeField] TrailRenderer m_trailRenderer = null;
    [SerializeField] float forceToAdd = 5f;
    [SerializeField] float shakeLenght = 0.32f;
    [SerializeField] int _steminaDeplete = 20;

    public int steminaDeplete{get{return _steminaDeplete;}}

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
                    if (attackableObj.isBlocking)
                    {
                        hitInfo[i].transform.LookAt(new Vector3(m_rootTranform.position.x,hitInfo[i].transform.position.y,m_rootTranform.position.z));

                        attackableObj?.TakeDamage(0,m_rootTranform.forward * forceToAdd);

                        if(hitInfo[i].CompareTag("Player"))
                        {
                            GameCore.m_GameContrller.SetNotControlableByTime(0.8f);
                            var player = hitInfo[i].GetComponent<Player>();
                            player.CharacterStemina.RemoveSP((int)(damagePerHit*1.5f));
                            player.Stop();
                            if(player.CharacterStemina.SP <= 10)
                            {
                                attackableObj?.TakeDamage(damagePerHit);
                            }
                        }
                    }
                    else
                    {
                        attackableObj?.TakeDamage(damagePerHit, m_rootTranform.forward * forceToAdd);
                    }
                }
                else
                {
                    attackableObj?.TakeDamage(damagePerHit);
                }
                if (shakeLenght != 0)
                {
                    GameCore.m_cameraController.ShakeCamera(0.22f, shakeLenght);
                }
            }
        }
    }
}
