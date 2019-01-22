using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField]Text m_txtCoin = null;

    public void UpdateTxtCoin(int value_u)
    {
        m_txtCoin.text = value_u.ToString();
    }
}
