using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    Health CharacterHP{get;}
    void TakeDamage(int damage);
    void Heal(int healValue);
}
