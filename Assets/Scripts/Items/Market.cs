using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField]GameObject[] itemsInMarket = null;

    public Inventory marketInventory {get;private set;}

    void Awake()
    {
        marketInventory = new Inventory(12);
    }
}
