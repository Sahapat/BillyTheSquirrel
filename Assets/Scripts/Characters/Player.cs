using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] int m_characterMaxHP = 100;

    [Header("Action Stamina Depletion")]
    [SerializeField] int NormalAttack = 20;
    [SerializeField] int HeavyAttack = 40;
    [SerializeField] int Dash = 20;

    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }

    private CapsuleCollider m_capsuleColider = null;
    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        CharacterStemina = GetComponent<Stemina>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
    }
    public bool CheckNormalAttackSP()
    {
        return CharacterStemina.SP >= NormalAttack;
    }
    public bool CheckHeavyAttackSP()
    {
        return CharacterStemina.SP >= HeavyAttack;
    }
    public bool CheckDashSP()
    {
        return CharacterStemina.SP >=Dash;
    }
    public void NormalAttackDepletion()
    {
        CharacterStemina.RemoveSP(NormalAttack);
    }
    public void HeavyAttackDepletion()
    {
        CharacterStemina.RemoveSP(HeavyAttack);
    }
    public void DashDepletion()
    {
        CharacterStemina.RemoveSP(Dash);
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