using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    [SerializeField]float timeToRespawn;
    [SerializeField]int MaxHP = 100;
    public Health CharacterHP{get;private set;}
    TakeDamageMaterialSetter m_takedamageSetter = null;
    float respawnCounter = 0f;
    bool DeadTriggerSet = false;

    private CapsuleCollider m_capsulecolider = null;
    private Rigidbody m_rigidbody = null;
    void Awake()
    {
        CharacterHP = new Health(MaxHP);
        m_takedamageSetter = GetComponent<TakeDamageMaterialSetter>();
        m_capsulecolider = GetComponent<CapsuleCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(CharacterHP.HP <= 0 && !DeadTriggerSet)
        {
            DeadTriggerSet = true;
            respawnCounter = Time.time + timeToRespawn;
            m_takedamageSetter.Die();
            m_capsulecolider.enabled = false;
            m_rigidbody.isKinematic = true;
        }
        if(DeadTriggerSet && respawnCounter <= Time.time)
        {
            m_takedamageSetter.Respawn();
            CharacterHP.ResetHP();
            DeadTriggerSet = false;
            m_capsulecolider.enabled = true;
            m_rigidbody.isKinematic = false;
        }
    }
    void Start()
    {
        CharacterHP.ResetHP();
    }
    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
    }

    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_takedamageSetter.TakeDamage(CharacterHP.HP,CharacterHP.MaxHP);
    }
}
