using System;
using UnityEngine;
using UnityEngine.Audio; // 이게 꼭 있어야 AudioMixer를 씁니다

public class BackManager : MonoBehaviour
{
    public static BackManager Instance { get; private set; }

    public event Action OnBackButtonPressed;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if(DataControl.LoadEncryptedDataFromPrefs("bgm_volume") != null) audioMixer.SetFloat("BGM", float.Parse(DataControl.LoadEncryptedDataFromPrefs("bgm_volume")));
        if(DataControl.LoadEncryptedDataFromPrefs("effect_volume") != null) audioMixer.SetFloat("Effect", float.Parse(DataControl.LoadEncryptedDataFromPrefs("bgm_volume")));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonPressed?.Invoke();
        }
    }
}