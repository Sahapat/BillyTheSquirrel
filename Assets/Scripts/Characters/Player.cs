using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] int m_characterMaxHP = 100;
    [SerializeField] LayerMask itemLayer = 0;
    [SerializeField] Transform WeaponHoldPos = null;
    [SerializeField] Transform ShieldHoldPos = null;

    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }

    private CapsuleCollider m_capsuleColider = null;
    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        CharacterStemina = GetComponent<Stemina>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
    }

    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
    }

    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
    }
}