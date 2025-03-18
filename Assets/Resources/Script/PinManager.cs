using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public PlayerStatus playerStatus; //핀 히트 수를 전달받을 스크립트

    public List<GameObject> pins = new List<GameObject>();
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
        playerStatus.PinHitCount = totalHits;
        return totalHits;
    }

    public void init_pins_hit_cnt(){
        foreach (var pinObject in pins){
            Pin pin = pinObject.GetComponent<Pin>();
            if (pin != null) pin.init_cnt();
        }
    }

    public void RespawnPins()
    {
        foreach (var pinObject in pins)
        {
            pinObject.SetActive(true); // 핀 다시 활성화
            Pin pin = pinObject.GetComponent<Pin>();
            if (pin != null)
            {
                pin.init_cnt(); // 핀 체력 초기화
            }
        }
    }

}
