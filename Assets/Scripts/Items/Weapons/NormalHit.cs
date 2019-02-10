using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHit : MonoBehaviour, IHitSystem
{
    [SerializeField]float m_delayForActive = 0f;
    [SerializeField]float m_delayForInActive = 0f;
    public float delayForActive
    {
        get
        {
            return m_delayForActive;
        }
        private set
        {
            m_delayForActive = value;
            m_delayForActive = Mathf.Clamp(m_delayForActive,0f,float.MaxValue);
        }
    }

    public float delayForInActive
    {
        get
        {
            return m_delayForInActive;
        }
        private set
        {
            m_delayForInActive = value;
            m_delayForInActive = Mathf.Clamp(m_delayForInActive,0f,float.MaxValue);
        }
    }
    private BoxCollider m_boxcolider = null;

    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    public void ActiveHit()
    {
        throw new System.NotImplementedException();
    }

    public void CancelHit()
    {
        throw new System.NotImplementedException();
    }
}
