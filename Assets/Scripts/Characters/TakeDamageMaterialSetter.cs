using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageMaterialSetter : MonoBehaviour
{
    private Renderer m_material = null;
    void Awake()
    {
        m_material = GetComponent<Renderer>();
    }
    public void TakeDamage(int currentHealth, int maxhealth)
    {
        var applyValue = (currentHealth * 1) / maxhealth;
        StopAllCoroutines();
        StartCoroutine(FadeOutWithColor(new Color(applyValue, 0, 0, 1), Color.white));
    }
    public void Die()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    public void Respawn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeOutWithColor(Color startColor, Color endColor)
    {
        m_material.material.color = startColor;
        while (true)
        {
            m_material.material.color = Color.Lerp(m_material.material.color, endColor, 0.2f);
            var remainDistance = Vector3.Distance(new Vector3(m_material.material.color.r, m_material.material.color.g, m_material.material.color.b),
            new Vector3(endColor.r, endColor.g, endColor.b));
            if (remainDistance < 0.01f)
            {
                break;
            }
            yield return null;
        }
        m_material.material.color = endColor;
    }
    IEnumerator FadeOut()
    {
        while (true)
        {
            m_material.material.color = Color.Lerp(m_material.material.color, Color.clear, 0.05f);
            var remainDistance = Vector3.Distance(new Vector3(m_material.material.color.r, m_material.material.color.g, m_material.material.color.b),
            new Vector3(Color.clear.r, Color.clear.g, Color.clear.b));
            if (remainDistance < 0.01f)
            {
                break;
            }
            yield return null;
        }
        m_material.material.color = Color.clear;
    }
    IEnumerator FadeIn()
    {
        while (true)
        {
            m_material.material.color = Color.Lerp(m_material.material.color, Color.white, 0.05f);
            var remainDistance = Vector3.Distance(new Vector3(m_material.material.color.r, m_material.material.color.g, m_material.material.color.b),
            new Vector3(Color.white.r, Color.white.g, Color.white.b));
            if (remainDistance < 0.01f)
            {
                break;
            }
            yield return null;
        }
        m_material.material.color = Color.white;
    }
}
