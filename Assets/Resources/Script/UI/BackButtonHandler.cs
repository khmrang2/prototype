using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonHandler : MonoBehaviour
{
    public GameObject exitPanel;
    private AudioSource AudioSource;

    private void Awake()
    {
        exitPanel.SetActive(false);
        AudioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }



    //private void OnEnable()
    //{
    //    BackManager.Instance.OnBackButtonPressed += HandleBackButton;
    //}

    //private void OnDisable()
    //{
    //    BackManager.Instance.OnBackButtonPressed -= HandleBackButton;
    //}

    private void HandleBackButton()
    {
        if (!exitPanel.activeSelf)
            exitPanel.SetActive(true);
        else
            exitPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        AudioSource.Play();
        exitPanel.SetActive(false);
    }
}
