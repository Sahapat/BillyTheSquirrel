using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]GameObject[] Doors;
    [SerializeField]float downoffsetSelf = 1f;
    [SerializeField]float speed = 5f;

    bool isOpen = false;

    private BoxCollider m_Boxcolider = null;
    private Vector3 destinationDoor;
    private Vector3 destinationSelf;
    void Awake()
    {
        m_Boxcolider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        if(isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position,destinationSelf,Time.deltaTime*speed);
        }
        var hitInfo = PhysicsExtensions.OverlapBox(m_Boxcolider,LayerMask.GetMask("Character"));

        if(hitInfo.Length > 0 && !isOpen)
        {
            isOpen = true;
            destinationSelf = transform.position + new Vector3(0,downoffsetSelf,0);
            foreach(GameObject temp in Doors)
            {
                temp.SetActive(false);
            }
        }
    }
}
