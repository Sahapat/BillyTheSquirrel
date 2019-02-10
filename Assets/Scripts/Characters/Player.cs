using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField]int m_characterMaxHP = 100;
    public Health CharacterHP{get; private set;}
    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
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
    }
}