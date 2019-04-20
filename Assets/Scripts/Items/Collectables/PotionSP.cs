using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSP : Item
{
    [SerializeField]int SP_Heal = 50;

    public override void Use(Player player)
    {
        player.CharacterStemina.AddSP(SP_Heal);
    }
}
