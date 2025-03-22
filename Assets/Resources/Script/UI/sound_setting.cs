using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // ✅ 이게 꼭 있어야 AudioMixer를 씁니다

public class volume_setting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(setbgm);
        effectSlider.onValueChanged.AddListener(seteffect);
    }

    public void setbgm(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void seteffect(float volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);
    }
}