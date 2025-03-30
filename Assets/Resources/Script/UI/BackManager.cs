using System;
using UnityEngine;

public class BackManager : MonoBehaviour
{
    public static BackManager Instance { get; private set; }

    public event Action OnBackButtonPressed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonPressed?.Invoke();
        }
    }
}