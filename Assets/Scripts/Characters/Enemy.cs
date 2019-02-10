using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    [SerializeField]int MaxHP = 100;
    public Health CharacterHP{get;private set;}

    void Awake()
    {
        CharacterHP = new Health(MaxHP);
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
