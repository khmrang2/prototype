using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public List<GameObject> pins = new List<GameObject>();
    public GameManager gameManager;

    public void Update(){
        if (gameManager.currentTurn == GameTurn.DropBallState && GameObject.Find("Ball") == null)
        {
            gameManager.damageSum = hit_cnt_sum();
            Debug.Log("Total pin hits: " + gameManager.damageSum);
            gameManager.ballHasDropped();
        }
    }
    // 각 Pin의 hit_cnt 값을 합산하여 반환
    public int hit_cnt_sum()
    {
        int totalHits = 0;

        foreach (var pinObject in pins)
        {
            Pin pin = pinObject.GetComponent<Pin>();
            if (pin != null)
            {
                totalHits += pin.hit_cnt();
            }
        }

        return totalHits;
    }
}
