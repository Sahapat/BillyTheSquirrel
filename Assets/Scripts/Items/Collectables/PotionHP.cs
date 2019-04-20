using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHP : Item
{
    [SerializeField]int HP_Heal = 50;

    public override void Use(Player player)
    {
        player.CharacterHP.AddHP(HP_Heal);
    }
}
