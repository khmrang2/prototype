using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private int cnt = 0;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 태그가 "ball"인지 확인
        if (collision.gameObject.CompareTag("Ball"))
        {
            // add_cnt 함수 호출
            add_cnt();
        }
    }

    private void add_cnt(){
        cnt++;
    }

    public int hit_cnt(){
        return cnt;
    }
    public void init_cnt(){
        cnt = 0;
    }
}
