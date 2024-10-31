using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    AudioSource myAudio;
    bool musicStart = false;
    private void Start()
    {
        myAudio = GetComponent<AudioSource>();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                myAudio.Play();
                musicStart = true;
            }
        }

    }
}
