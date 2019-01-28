using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]GameObject txt3D= null;
    private BoxCollider m_boxColider2D;

    void Awake()
    {
        m_boxColider2D = GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            txt3D.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            txt3D.SetActive(false);
        }
    }
    public GameObject PickObjUp()
    {
        m_boxColider2D.enabled = false;
        txt3D.SetActive(false);
        return this.transform.parent.gameObject;
    }
}
