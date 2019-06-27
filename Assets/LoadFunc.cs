using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadFunc : MonoBehaviour
{
    Slider slider;
    void Awake()
    {
        slider = transform.Find("Slider").GetComponent<Slider>();
        StartCoroutine(LoadManager.Instance.LoadScene().StartLoading());
    }

    private void Update()
    {
        slider.value = LoadManager.Instance.LoadScene().GetProgress();
    }

}