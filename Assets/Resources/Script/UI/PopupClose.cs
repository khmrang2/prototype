using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClose : MonoBehaviour
{

    public AudioSource close_button_click_sound;
    public void ClosePopup()
    {
        PlayOneShotSound(close_button_click_sound);
        this.gameObject.SetActive(false);
    }
    
    private void PlayOneShotSound(AudioSource source)
    {
        if (source == null || source.clip == null) return;

        // 임시 오브젝트 생성
        GameObject tempAudioObj = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioObj.AddComponent<AudioSource>();
        tempAudio.clip = source.clip;
        tempAudio.outputAudioMixerGroup = source.outputAudioMixerGroup; // 믹서 연결 유지
        tempAudio.volume = source.volume;
        tempAudio.spatialBlend = 0f; // 2D
        tempAudio.Play();

        Destroy(tempAudioObj, tempAudio.clip.length);
    }
}
