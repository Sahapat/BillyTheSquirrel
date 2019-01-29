using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    private Image m_showImage = null;

    void Awake()
    {
        m_showImage = GetComponentInChildren<Image>();
    }
    public void SetImage(Item m_itemIn)
    {
        m_showImage.sprite = m_itemIn.midmapIcon;
        print(m_itemIn.discription);
    }
    public void Clear()
    {
        m_showImage.sprite = null;
    }
}
