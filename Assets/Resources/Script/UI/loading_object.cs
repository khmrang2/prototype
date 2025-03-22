using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loading_object : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            Debug.Log("play!!!!!!");
            audioSource.Play();
        }
    }
}
