using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// °ÔÀÓÀÇ °¢ »óÈ²À» Á¤ÀÇÇÏ°í Á¦¾îÇÏ±â À§ÇØ¼­
// ±×³É ÀÌ¸§ ÁöÀº enum.
public enum GameTurn
{
    // 11ÀÏ (¿ù) ºñ´ë¸é È¸ÀÇ + ÀÌ½´ Ã¼Å© È¸ÀÇ
    // 
    // 13ÀÏ (¼ö) ¸àÅä´Ô ÄÁÆß
    // ÅÂ¿¬
    DropBallState,          // ÇÃ·¹ÀÌ¾îÀÇ ÅÏÀ¸·Î °øÀ» ¶³¾î¶ß¸®´Â »óÅÂ - ÅÂ¿¬

    // Á¤ÈÆ´Ô 
    PlayerAtkState,         // ¶³¾î¶ß¸° °øÀ¸·Î ÀûÀ» °ø°İÇÏ´Â »óÅÂ

    // ½Ã¿ì´Ô
    EnemyBehaviorState,     // ÀûÀÇ ÅÏÀ¸·Î ÀûÀÌ Çàµ¿(°ø°İ or ¿òÁ÷ÀÓ)ÇÏ´Â »óÅÂ
    SpawnEnemyState,        // ÀûÀÌ »ı¼ºµÇ´Â »óÅÂ

    // Çö¹Î
    EndChkState,            // ½ºÅ×ÀÌÁö°¡ ³¡³µ´ÂÁö(¸ğµç ÀûÀÌ Á×¾ú´ÂÁö) Ã¼Å©ÇÏ´Â »óÅÂ
    ChooseBuffState,        // ÇÃ·¹ÀÌ¾îÀÇ ÅÏÀ¸·Î ¹öÇÁ¸¦ ¼±ÅÃÇÏ´Â »óÅÂ->>
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
    public Transform playerTransform;  // ÇÃ·¹ÀÌ¾îÀÇ Transform
    public EnemyListManager enemyListManager;  // EnemyListManager ÂüÁ¶
    // Çö¹Î - 
    // °ÔÀÓÇÒ stateµéÀ» ºÒ·¯¿È.
    // ÇÃ·¹ÀÌ¾î°¡ ±âº»ÀûÀ¸·Î ºÒ·¯¿À´Â state. 
    // ¹öÇÁ¸¦ ¹Ş¾Æ¼­ °»½ÅµÉ cur_state.
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
        // °ÔÀÓÀ» ½ÃÀÛÇÒ ÇÁ·¹ÀÓ ¿öÅ©ÀÇ ½ÃÀÛ.
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            switch (currentTurn)
            {
                case GameTurn.DropBallState:
                    yield return StartCoroutine(DropBallTurn());
                    currentTurn = GameTurn.PlayerAtkState;
                    break;
                case GameTurn.PlayerAtkState:
                    yield return StartCoroutine(PlayerAtkTurn());
                    currentTurn = GameTurn.EnemyBehaviorState;
                    break;
                case GameTurn.EnemyBehaviorState:
                    yield return StartCoroutine(EnemyBehaviorTurn());
                    currentTurn = GameTurn.SpawnEnemyState;
                    break;
                case GameTurn.SpawnEnemyState:
                    yield return StartCoroutine(SpawnEnemyTurn());
                    currentTurn = GameTurn.ChooseBuffState;
                    break;
                case GameTurn.ChooseBuffState:
                    yield return StartCoroutine(ChooseBuffTurn());
                    currentTurn = GameTurn.EndChkState;
                    break;
                case GameTurn.EndChkState:
                    yield return StartCoroutine(EndChkStage());
                    currentTurn = GameTurn.DropBallState;
                    break;
            }
        }
    }

    private IEnumerator DropBallTurn()
    {
        Debug.Log("Dropping ball...");
        yield return new WaitUntil(() => ballHasDropped());
    }

    private IEnumerator PlayerAtkTurn()
    {
        //ÇÃ·¹ÀÌ¾î °ø°İ Åõ»çÃ¼ »ı¼º
        //Åõ»çÃ¼´Â ½º½º·Î ³ª¾Æ°¡¸ç Àû°ú Á¢ÃËÇÏ°Å³ª ÁöÁ¤ÇÑ ¹üÀ§ ¹ÛÀ¸·Î ³ª°¡¸é ½º½º·Î Á¦°Å
        plAtkObj = Instantiate(prefPlayerAtkProjrctile);
        plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
        Debug.Log("Player attacking...");
        yield return new WaitUntil(() => enemyAtkEnded());
    }

    private IEnumerator EnemyBehaviorTurn()
    {
        Debug.Log("Enemy Behavior...");
        // ÀûÀÇ Çàµ¿ (°ø°İ ¶Ç´Â ÀÌµ¿)À» Ã³¸®ÇÏ´Â ·ÎÁ÷
        enemyListManager.HandleEnemyBehavior();  // ÀûÀÌ ÇÃ·¹ÀÌ¾î¸¦ ÇâÇØ ÀÌµ¿ÇÏ°Å³ª °ø°İÇÏµµ·Ï Ã³¸®
        yield return new WaitUntil(() => enemyMoveEnded());
    }

    private IEnumerator SpawnEnemyTurn()
    {
        Debug.Log("Spawning enemies...");
        // 5ÃÊ °£°İÀ¸·Î Àû 5¸íÀ» ¼ÒÈ¯
        enemyListManager.SpawnEnemiesWithInterval();
        yield return new WaitUntil(() => spawnEnemyEnded());
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("Choosing a buff...");
        buffManager.ShowBuffSelection(); // ¹öÇÁ ¼±ÅÃ UI Ç¥½Ã

        // ¹öÇÁ°¡ ¼±ÅÃµÉ ¶§±îÁö ´ë±â
        yield return new WaitUntil(() => buffManager.IsBuffSelected());
        updateBuffState();
        Debug.Log("¹öÇÁ ¾÷µ¥ÀÌÆ®µÊ.");
        buffState.printAllStates();
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        damageSum = 0;
        interactionArea.init_ball();
        yield return new WaitUntil(() => chkStageEnded());
    }

    public bool ballHasDropped()
{
    // ê³µì´ ?¨ì–´ì§€ê³??ˆëŠ” ì¤‘ì´ê³? Ball ?¤ë¸Œ?íŠ¸ê°€ ???´ìƒ ì¡´ì¬?˜ì? ?Šìœ¼ë©?
    if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
    {
        // pinManager?ì„œ ?©ì‚°??hit countë¥?damageSum???€??
        damageSum = pinManager.hit_cnt_sum();
        Debug.Log("Total hit count: " + damageSum);

        // ëª¨ë“  ?€??hit count ì´ˆê¸°??
        pinManager.init_pins_hit_cnt();

        // true ë°˜í™˜
        return true;
    }
    return false;
}
    private bool enemyAtkEnded()
    {
        if (plAtkObj == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool buffChosen()
    {
        return true;
    }

    private bool enemyMoveEnded()
    {
        return true; 
    }

    private bool spawnEnemyEnded()
    {
        return true;
    }

    private bool chooseBuffEnded()
    {
        // ÀÌÇÏµ¿¹® 
        // À¯Àú°¡ Å¬¸¯ÇÏ´Â ÆĞ³ÎÀÌ »ı¼ºµÇ°í
        // ÆĞ³Î¿¡¼­ ¼±ÅÃÇÑ ¹öÆ°´ë·Î ¹öÇÁ ¸Å´ÏÀú¿¡¼­ update°¡ µÉ°ÅÀÓ.
        // ±×·³ ÀÌÁ¦ ¹öÆ®¸Å´ÏÀú¿¡¼­ °¡Á®¿À´Â °ÍÀÌ ÇÊ¿äÇÏ³×?
        // Áï, ¹öÆ°ÀÌ Å¬¸¯µÇ°í updateBuffState()°¡ ½ÇÇàµÇ¸é return À¸·Î 1 ¾Æ´Ï¸é 0 
        //curState = buffManager.getBuffState();
        return true;
    }

    private bool chkStageEnded()
    {
        return true;  // ½ºÅ×ÀÌÁö°¡ ³¡³µ´ÂÁö¸¦ Ã¼Å©ÇÏ´Â ·ÎÁ÷
    }

    public void updateBuffState()
    {
        this.buffState = buffManager.getBuffSumState();
    }
}