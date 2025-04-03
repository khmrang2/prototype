using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // 이게 꼭 있어야 AudioMixer를 씁니다

public class volume_setting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    private void Awake()
    {
        if (DataControl.LoadEncryptedDataFromPrefs("bgm_volume") != null)
        {
            float bgmDb = float.Parse(DataControl.LoadEncryptedDataFromPrefs("bgm_volume"));
            bgmSlider.value = Mathf.Pow(10f, bgmDb / 20f);
        }

        if (DataControl.LoadEncryptedDataFromPrefs("effect_volume") != null)
        {
            float effectDb = float.Parse(DataControl.LoadEncryptedDataFromPrefs("effect_volume"));
            effectSlider.value = Mathf.Pow(10f, effectDb / 20f);
        }
		bgmSlider.onValueChanged.AddListener(setbgm);
        effectSlider.onValueChanged.AddListener(seteffect);
    }

    public void setbgm(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        DataControl.SaveEncryptedDataToPrefs("bgm_volume", (Mathf.Log10(volume) * 20).ToString());
    }

    public void seteffect(float volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);
        DataControl.SaveEncryptedDataToPrefs("effect_volume", (Mathf.Log10(volume) * 20).ToString());
    }
}