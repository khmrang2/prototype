using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½È²ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï±ï¿½ ï¿½ï¿½ï¿½Ø¼ï¿½
// ï¿½×³ï¿½ ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½ enum.
public enum GameTurn
{
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
        enemyListManager.SpawnInitialEnemies();
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
        Debug.Log("Enemies moving...");
        enemyListManager.MoveEnemies(); // ÀûÀÌ ÀÌµ¿ÇÏµµ·Ï È£Ãâ

        yield return new WaitUntil(() => enemyMoveEnded());
        currentTurn = GameTurn.ChooseBuffState;
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
        if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
        {
            // pinManager?ì„œ ?©ì‚°??hit countë¥?damageSum???€??
            damageSum = pinManager.hit_cnt_sum();
            Debug.Log("Total hit count: " + damageSum);

            pinManager.init_pins_hit_cnt();

            // true ë°˜í™˜
            return true;
        }
        return false;
    }
    private bool enemyAtkEnded()
    {
        return plAtkObj == null;
    }

    private bool buffChosen()
    {
        return true;
    }

    private bool enemyMoveEnded()
    {
        return enemyListManager.AllEnemiesMoved();
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

    // Åõ»çÃ¼°¡ Á¦°ÅµÉ ¶§ GameManager¿¡ ¾Ë¸²
    public void NotifyProjectileDestroyed()
    {
        plAtkObj = null;
    }
}