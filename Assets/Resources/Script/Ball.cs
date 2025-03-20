using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ball : MonoBehaviour
{
    private PlayerState playerState;

    [SerializeField] private Transform ball;
    [SerializeField] private PhysicsMaterial2D ball_elacity;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Awake()
    {
        if (playerState == null)
        {
            playerState = FindObjectOfType<PlayerState>();
        }
        float scaleMultiplier = 1f + (playerState.Ball_Size / 100f);
        // 기존 스케일에 multiplier를 곱합니다.
        ball.localScale = ball.localScale * scaleMultiplier;

        ball_elacity.bounciness = playerState.Ball_Elasticity;
    }
}
