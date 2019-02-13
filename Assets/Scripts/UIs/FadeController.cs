using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    [SerializeField] float fadingTime = 0.8f;
    private Image m_fadeImage = null;

    private int sceneToLoad = 0;
    private GameObject f_obj = null;
    private GameObject s_obj = null;
    private GameObject loadingObject = null;
    private Slider loadingSlider = null;
    void Awake()
    {
        m_fadeImage = GetComponent<Image>();
        loadingObject= transform.Find("LoadingScene").gameObject;
        loadingObject.SetActive(true);
        loadingSlider = GetComponentInChildren<Slider>();
        loadingObject.SetActive(false);
        DoFadeIn();
    }
    public void DoFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    public void DoFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    public void LoadSceneAndFade(SceneInfo info)
    {
        sceneToLoad = (int)info;
        DoFadeOut();
        Invoke("LoadScene",fadingTime);
    }
    public void LoadSceneAndFade(int info)
    {
        sceneToLoad = info;
        DoFadeOut();
        Invoke("LoadScene",fadingTime);
    }
    public void SwitchAndFade(GameObject startObj,GameObject secondObj)
    {
        DoFadeOut();
        Invoke("SwitchObj",fadingTime);
    }
    private void SwitchObj()
    {
        f_obj.SetActive(false);
        s_obj.SetActive(true);
        DoFadeIn();
    }
    private void LoadScene()
    {
        loadingObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(LoadingScene());
    }
    private IEnumerator FadeIn()
    {
        float timeStart = Time.time;
        Color targetColor = new Color(m_fadeImage.color.r,m_fadeImage.color.g,m_fadeImage.color.b,0);
        m_fadeImage.color = new Color(targetColor.r,targetColor.g,targetColor.b,1);

        while(m_fadeImage.color.a > 0.1f)
        {
            m_fadeImage.color = Color.Lerp(m_fadeImage.color,targetColor,Time.deltaTime * 4f);
            yield return null;
        }
        m_fadeImage.color = targetColor;
    }
    private IEnumerator FadeOut()
    {
        float timeStart = Time.time;
        Color targetColor = new Color(m_fadeImage.color.r,m_fadeImage.color.g,m_fadeImage.color.b,1);
        m_fadeImage.color = new Color(targetColor.r,targetColor.g,targetColor.b,0);
        while(m_fadeImage.color.a < 0.9f)
        {
            m_fadeImage.color = Color.Lerp(m_fadeImage.color,targetColor,Time.deltaTime * 4f);
            yield return null;
        }
        m_fadeImage.color = targetColor;
    }
    private IEnumerator LoadingScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        while(!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress/0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
