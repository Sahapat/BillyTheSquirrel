using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    /* [SerializeField] GameObject[] PopOutObjects = null;
    Animator chestAnim = null;
    BoxCollider m_boxcolider = null;
    bool isPopOut = false;
    void Awake()
    {
        chestAnim = GetComponentInChildren<Animator>();
        m_boxcolider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        if (isPopOut) return;
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Character"));

        if (hitInfo.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                isPopOut = true;
                chestAnim.SetTrigger("Open");
                Invoke("PopOut", 2f);
            }
        }
    }
    void PopOut()
    {
        for (int i = 0; i < PopOutObjects.Length; i++)
        {
            var popOutPosition = transform.position + new Vector3(0, 0.5f, 0);
            var temp = Instantiate(PopOutObjects[i], popOutPosition, Quaternion.identity);
            var XAddValue = Random.Range(-1, 1) * Random.value;
            var ZAddValue = transform.forward.z * 0.2f;
            temp.GetComponent<IPopable>().PopOut(XAddValue, ZAddValue, 5f);
        }
        Destroy(this.gameObject);
    } */
}
