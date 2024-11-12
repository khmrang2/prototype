using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ÔøΩÔøΩÔøΩÔøΩÔøΩÔøΩ ÔøΩÔøΩ ÔøΩÔøΩ»≤ÔøΩÔøΩ ÔøΩÔøΩÔøΩÔøΩÔøΩœ∞ÔøΩ ÔøΩÔøΩÔøΩÔøΩÔøΩœ±ÔøΩ ÔøΩÔøΩÔøΩÿºÔøΩ
// ÔøΩ◊≥ÔøΩ ÔøΩÃ∏ÔøΩ ÔøΩÔøΩÔøΩÔøΩ enum.
public enum GameTurn
{
    // 13¿œ (ºˆ) ∏‡≈‰¥‘ ƒ¡∆ﬂ
    // ≈¬ø¨
    DropBallState,          // «√∑π¿ÃæÓ¿« ≈œ¿∏∑Œ ∞¯¿ª ∂≥æÓ∂ﬂ∏Æ¥¬ ªÛ≈¬ - ≈¬ø¨

    // ¡§»∆¥‘ 
    PlayerAtkState,         // ∂≥æÓ∂ﬂ∏∞ ∞¯¿∏∑Œ ¿˚¿ª ∞¯∞›«œ¥¬ ªÛ≈¬

    // Ω√øÏ¥‘
    EnemyBehaviorState,     // ¿˚¿« ≈œ¿∏∑Œ ¿˚¿Ã «‡µø(∞¯∞› or øÚ¡˜¿”)«œ¥¬ ªÛ≈¬
    SpawnEnemyState,        // ¿˚¿Ã ª˝º∫µ«¥¬ ªÛ≈¬

    // «ˆπŒ
    EndChkState,            // Ω∫≈◊¿Ã¡ˆ∞° ≥°≥µ¥¬¡ˆ(∏µÁ ¿˚¿Ã ¡◊æ˙¥¬¡ˆ) √º≈©«œ¥¬ ªÛ≈¬
    ChooseBuffState,        // «√∑π¿ÃæÓ¿« ≈œ¿∏∑Œ πˆ«¡∏¶ º±≈√«œ¥¬ ªÛ≈¬->>
}
struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};
public class GameManager : MonoBehaviour
{
    public GameObject prefPlayerAtkProjrctile;
    private GameObject plAtkObj;
    public Transform playerTransform;  // «√∑π¿ÃæÓ¿« Transform
    public EnemyListManager enemyListManager;  // EnemyListManager ¬¸¡∂
    // «ˆπŒ - 
    // ∞‘¿”«“ stateµÈ¿ª ∫“∑Øø».
    // «√∑π¿ÃæÓ∞° ±‚∫ª¿˚¿∏∑Œ ∫“∑Øø¿¥¬ state. 
    // πˆ«¡∏¶ πﬁæ∆º≠ ∞ªΩ≈µ… cur_state.
    [SerializeField]
    public BuffManager buffManager;
    
    BaseState buffState = null;
    BaseState defaultState = null;
    BaseState playerState = null;

    public int damageSum = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;
    public PinManager pinManager;
    public InteractionArea interactionArea;

    void Start()
    {
        buffState = new BaseState();
        defaultState = new BaseState();
        playerState = new BaseState();
        // ∞‘¿”¿ª Ω√¿€«“ «¡∑π¿” øˆ≈©¿« Ω√¿€.
    }

    void Update()
    {
        switch (currentTurn)
        {
            case GameTurn.DropBallState:
                if (ballHasDropped())
                {
                    currentTurn = GameTurn.PlayerAtkState;
                }
                break;

            case GameTurn.PlayerAtkState:
                if (plAtkObj == null)
                {
                    // ≈ıªÁ√º ª˝º∫
                    plAtkObj = Instantiate(prefPlayerAtkProjrctile);
                    plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
                    Debug.Log("Player attacking...");
                }

                if (enemyAtkEnded())
                {
                    currentTurn = GameTurn.EnemyBehaviorState;
                }
                break;

            case GameTurn.EnemyBehaviorState:
                Debug.Log("Enemy Behavior...");
                enemyListManager.HandleEnemyBehavior();

                if (enemyMoveEnded())
                {
                    currentTurn = GameTurn.SpawnEnemyState;
                }
                break;

            case GameTurn.SpawnEnemyState:
                Debug.Log("Spawning enemies...");
                enemyListManager.SpawnEnemiesWithInterval();

                if (spawnEnemyEnded())
                {
                    currentTurn = GameTurn.ChooseBuffState;
                }
                break;

            case GameTurn.ChooseBuffState:
                Debug.Log("Choosing a buff...");
                buffManager.ShowBuffSelection();

                if (buffManager.IsBuffSelected())
                {
                    updateBuffState();
                    Debug.Log("πˆ«¡ æ˜µ•¿Ã∆Æµ .");
                    currentTurn = GameTurn.EndChkState;
                }
                break;

            case GameTurn.EndChkState:
                Debug.Log("Checking end conditions...");
                damageSum = 0;
                interactionArea.init_ball();

                if (chkStageEnded())
                {
                    currentTurn = GameTurn.DropBallState;
                }
                break;
        }
    }

    private bool ballHasDropped()
    {
        if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
        {
            damageSum = pinManager.hit_cnt_sum();
            Debug.Log("Total hit count: " + damageSum);
            pinManager.init_pins_hit_cnt();
            return true;
        }
        return false;
    }

    private bool enemyAtkEnded()
    {
        return plAtkObj == null;
    }

    private bool enemyMoveEnded()
    {
        return true;
    }

    private bool spawnEnemyEnded()
    {
        return true;
    }

    private bool chkStageEnded()
    {
        return true;
    }

    public void updateBuffState()
    {
        buffState = buffManager.getBuffSumState();
    }

    //private void DropBallTurn()
    //{
    //    Debug.Log("Dropping ball...");
    //    if (ballHasDropped())
    //    {
    //        ChangeTurn(GameTurn.PlayerAtkState);
    //    }
    //}

    //private void PlayerAtkTurn()
    //{
    //    if (plAtkObj == null)
    //    {
    //        plAtkObj = Instantiate(prefPlayerAtkProjrctile);
    //        plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
    //        Debug.Log("Player attacking...");
    //    }

    //    if (enemyAtkEnded())
    //    {
    //        ChangeTurn(GameTurn.EnemyBehaviorState);
    //    }
    //}

    //private void EnemyBehaviorTurn()
    //{
    //    Debug.Log("Enemy Behavior...");
    //    enemyListManager.HandleEnemyBehavior();

    //    if (enemyMoveEnded())
    //    {
    //        ChangeTurn(GameTurn.SpawnEnemyState);
    //    }
    //}

    //private void SpawnEnemyTurn()
    //{
    //    Debug.Log("Spawning enemies...");
    //    enemyListManager.SpawnEnemiesWithInterval();

    //    if (spawnEnemyEnded())
    //    {
    //        ChangeTurn(GameTurn.ChooseBuffState);
    //    }
    //}

    //private void ChooseBuffTurn()
    //{
    //    Debug.Log("Choosing a buff...");
    //    buffManager.ShowBuffSelection();

    //    if (buffManager.IsBuffSelected())
    //    {
    //        updateBuffState();
    //        Debug.Log("πˆ«¡ æ˜µ•¿Ã∆Æµ .");
    //        buffState.printAllStates();
    //        ChangeTurn(GameTurn.EndChkState);
    //    }
    //}

    //private void EndChkStage()
    //{
    //    Debug.Log("Checking end conditions...");
    //    damageSum = 0;
    //    interactionArea.init_ball();

    //    if (chkStageEnded())
    //    {
    //        ChangeTurn(GameTurn.DropBallState);
    //    }
    //}

    //public bool ballHasDropped()
    //{
    //    if(interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
    //    {
    //        // pinManager?êÏÑú ?©ÏÇ∞??hit countÎ•?damageSum???Ä??
    //        damageSum = pinManager.hit_cnt_sum();
    //        Debug.Log("Total hit count: " + damageSum);

    //        pinManager.init_pins_hit_cnt();

    //        // true Î∞òÌôò
    //        return true;
    //    }
    //    return false;
    //}
    //private bool enemyAtkEnded()
    //{
    //    if (plAtkObj == null)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private bool buffChosen()
    //{
    //    return true;
    //}

    //private bool enemyMoveEnded()
    //{
    //    return true; 
    //}

    //private bool spawnEnemyEnded()
    //{
    //    return true;
    //}

    //private bool chooseBuffEnded()
    //{
    //    // ¿Ã«œµøπÆ 
    //    // ¿Ø¿˙∞° ≈¨∏Ø«œ¥¬ ∆–≥Œ¿Ã ª˝º∫µ«∞Ì
    //    // ∆–≥Œø°º≠ º±≈√«— πˆ∆∞¥Î∑Œ πˆ«¡ ∏≈¥œ¿˙ø°º≠ update∞° µ…∞≈¿”.
    //    // ±◊∑≥ ¿Ã¡¶ πˆ∆Æ∏≈¥œ¿˙ø°º≠ ∞°¡Æø¿¥¬ ∞Õ¿Ã « ø‰«œ≥◊?
    //    // ¡Ô, πˆ∆∞¿Ã ≈¨∏Øµ«∞Ì updateBuffState()∞° Ω««‡µ«∏È return ¿∏∑Œ 1 æ∆¥œ∏È 0 
    //    //curState = buffManager.getBuffState();
    //    return true;
    //}

    //private bool chkStageEnded()
    //{
    //    return true;  // Ω∫≈◊¿Ã¡ˆ∞° ≥°≥µ¥¬¡ˆ∏¶ √º≈©«œ¥¬ ∑Œ¡˜
    //}

    //public void updateBuffState()
    //{
    //    this.buffState = buffManager.getBuffSumState();
    //}
}