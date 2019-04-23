using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingRandom : MonoBehaviour
{
    [SerializeField]Sprite[] randoms = null;
    void OnEnable()
    {
        GetComponent<Image>().sprite = randoms[Random.Range(0,randoms.Length)];
    }
}
