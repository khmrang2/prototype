using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Destroy(gameObject); // 씬이 변경되면 자동 삭제
    }

}
