using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMaterial : MonoBehaviour
{
    //[SerializeField]Color endColor = Color.red;
    [SerializeField] Renderer[] m_renderer = null;
    [SerializeField] GameObject[] specialRenderers = null;

    /* Color[] defaultColor = null;
    void Start()
    {
        defaultColor = new Color[m_renderer.Length];
        for(int i =0;i<m_renderer.Length;i++)
        {
            defaultColor[i] = m_renderer[i].material.color;
        }
    }
    void Update()
    {
        foreach(var iterate in m_renderer)
        {
            iterate.material.color = endColor;
        }
    } */
    public void FadeOut()
    {
        DoFadeOut();
    }
    public void FadeOut(float delayTime)
    {
        Invoke("DoFadeOut",delayTime);
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
    }
}
