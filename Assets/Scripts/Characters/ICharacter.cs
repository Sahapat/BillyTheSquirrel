using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    Health CharacterHP{get;}
    void TakeDamage(int damage);
    void TakeDamage(int damage,Vector3 forceToAdd);
}
