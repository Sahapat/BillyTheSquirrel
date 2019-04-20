using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
    [SerializeField] int _coinValue = 1;
    public int coinValue { get { return _coinValue; } }

    private BoxCollider m_boxcolider = null;
    private PopOutController m_Controller = null;

    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void FixedUpdate()
    {
        var root = transform.root;
        if (root.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            if(!m_Controller)m_Controller = root.GetComponent<PopOutController>();
            if (m_Controller.isFinishPopOut)
            {
                var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Character"));

                if (hitInfo.Length > 0)
                {
                    GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.AddCoin(coinValue);

                    Destroy(this.transform.root.gameObject);
                }
            }
        }
        else
        {
            var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Character"));

                if (hitInfo.Length > 0)
                {
                    GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.AddCoin(coinValue);

                    Destroy(this.transform.root.gameObject);
                }
        }
    }
}
