using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHitForGreatSword : HeavyHit
{
    [SerializeField]BoxCollider hitColiderChecker = null;
    [SerializeField]TrailRenderer m_trailRenderer = null;

    private MeshRenderer hitMeshrenderer = null;


    void Start()
    {
        hitMeshrenderer = hitColiderChecker.GetComponent<MeshRenderer>();
        m_trailRenderer.enabled = false;
    }
    void Update()
    {
        if(hitColiderChecker)
        {
            var root = this.transform.root.gameObject;
            if(root.CompareTag("Player") || root.CompareTag("Enemy") || root.CompareTag("Boss"))
            {
                hitColiderChecker.transform.parent = root.transform;
                hitColiderChecker.transform.localPosition = coliderPositionLocalToRoot;
                hitColiderChecker.transform.localRotation = Quaternion.Euler(coliderRotateLocalToRoot);
            }
        }
    }
    protected override void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(hitColiderChecker, TargetLayer);
        if (hitInfo.Length == 0) return;
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var attackableObj = hitInfo[i].GetComponent<IAttackable>();
                if (hitInfo[i].CompareTag("Player") || hitInfo[i].CompareTag("Enemy"))
                {
                    var addForceDirection = -hitInfo[i].transform.forward;
                    addForceDirection.y = 0;
                    addForceDirection *= forceToAdd;
                    attackableObj?.TakeDamage(damagePerHit,addForceDirection);
                }
                else
                {
                    attackableObj?.TakeDamage(damagePerHit);
                }
                if(shakeLenght != 0)
                {
                    GameCore.m_cameraController.ShakeCamera(0.22f,shakeLenght);
                }
            }
        }
    }
    protected override void OnActive()
    {
        hitMeshrenderer.enabled = true;
    }
    protected override void OnInActive()
    {
        hitMeshrenderer.enabled = false;
    }
    protected override void OnResetHit()
    {
        m_trailRenderer.enabled = false;
    }
    public void ActiveTrail()
    {
        m_trailRenderer.enabled = true;
    }
}
