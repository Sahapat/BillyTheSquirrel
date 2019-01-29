using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField]Text m_txtCoin = null;
    [SerializeField]Text m_txtHealth = null;
    [SerializeField]Text m_txtStemina = null;

    public void UpdateTxtCoin(int value_u)
    {
        m_txtCoin.text = value_u.ToString();
    }
    public void UpdateTxtHealth(int value_u)
    {
        m_txtHealth.text = value_u.ToString();
    }
    public void UpdateTxtStemina(int value_u)
    {
        m_txtStemina.text = value_u.ToString();
    }
}
