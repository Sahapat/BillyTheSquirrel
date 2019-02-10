using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory
{
    [SerializeField]int _maxSlot = 0;
    public int maxSlot
    {
        get
        {
            return _maxSlot;
        }
        private set
        {
            _maxSlot = value;
        }
    }


}
