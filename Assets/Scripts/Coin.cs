using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]int coinValue = 1;
    [SerializeField]float spinValue = 1;

    void Update()
    {
        transform.Rotate(Vector3.up*spinValue);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameCore.m_Main.Coin+=coinValue;
            Destroy(this.gameObject);
        }
    }
}
