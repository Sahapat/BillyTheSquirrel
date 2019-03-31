using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMaterial : MonoBehaviour
{
    [SerializeField] Renderer[] m_renderer = null;
    [SerializeField] GameObject[] specialRenderers = null;

    private Color[] defaultColor = null;

    private WaitForSeconds waitForBackToDefault = null;
    void Awake()
    {
        waitForBackToDefault = new WaitForSeconds(0.5f);
        defaultColor = new Color[m_renderer.Length];
        for(int i =0;i<m_renderer.Length;i++)
        {
            defaultColor[i] = m_renderer[i].material.color;
        }
    }
    void DoFadeOut()
    {
        SetTranperentMaterial();
        foreach (var i in m_renderer)
        {
            iTween.FadeTo(i.gameObject, 0, 2);
        }
        Invoke("SetInVisibleMaterial", 2f);
    }
    void SetTranperentMaterial()
    {
        foreach (var i in m_renderer)
        {
            i.material.SetInt("_ScrBlend", (int)UnityEngine.Rendering.BlendMode.One);
            i.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            i.material.SetInt("_ZWrite", 0);
            i.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            i.material.DisableKeyword("_ALPHABLEND_ON");
            i.material.DisableKeyword("_ALPHATEST_ON");
            i.material.renderQueue = 3000;
        }
        foreach (var i in specialRenderers)
        {
            i.SetActive(false);
        }
    }
    void SetInVisibleMaterial()
    {
        foreach (var i in m_renderer)
        {
            i.material.renderQueue = 1;
        }
        Destroy(this.gameObject);
    }
    public void TakeDamageMaterialActive(int currentHealth,int maxHealth)
    {
        StopAllCoroutines();
        foreach(var i in m_renderer)
        {
            iTween.Stop(i.gameObject);
        }
        float getPercentageToMaxHealth = (currentHealth*100)/maxHealth;
        getPercentageToMaxHealth*=0.01f;
        Color materialByHealthColor = new Color(1,getPercentageToMaxHealth,getPercentageToMaxHealth,1);
        foreach(var i in m_renderer)
        {
            i.material.color = materialByHealthColor;
        }
        StartCoroutine(DoBackToDefault());
    }
    public void FadeOut()
    {
        DoFadeOut();
    }
    public void FadeOut(float delayTime)
    {
        Invoke("DoFadeOut",delayTime);
    }
    private IEnumerator DoBackToDefault()
    {
        yield return waitForBackToDefault;
        for(int i=0;i<defaultColor.Length;i++)
        {
            iTween.ColorTo(m_renderer[i].gameObject,defaultColor[i],0.3f);
        }
    }
}
