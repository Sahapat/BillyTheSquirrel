using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall_Color : MonoBehaviour
{
    [SerializeField] Color startColor = Color.white;
    [SerializeField] Color endColor = Color.black;
    [SerializeField] float blinkDuration = 2f;

    private float counterForBlink = 0f;
    private Material m_material = null;

    void Awake()
    {
        m_material = GetComponent<Renderer>().material;
    }
}
