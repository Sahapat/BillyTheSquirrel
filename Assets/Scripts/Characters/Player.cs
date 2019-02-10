using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField]int m_characterMaxHP = 100;
    [SerializeField]int m_characterMaxSP = 100;
    public Health CharacterHP{get; private set;}

    public Stemina CharacterSP{get;private set;}

    private delegate void updateStatusUI();
    void Awake()
    {
    }
    void Start()
    {
        CharacterHP.MaxHP = m_characterMaxHP;
        CharacterSP.MaxSP = m_characterMaxSP;
        CharacterHP.ResetHP();
        CharacterSP.ResetSP();
    }
    public void Heal(int healValue)
    {
        CharacterHP.HP += healValue;
    }

    public void TakeDamage(int damage)
    {
        CharacterHP.HP -= damage;
    }
}