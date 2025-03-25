using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class ProjectileOnHit : MonoBehaviour
{
    public AudioSource player_attack_sound;
    public AudioSource enemy_hit_sound;
    [Header("Projrctile Variables")]
    public bool isDestroyed;
    public Transform returnTransform;
    public Transform InstantiateTransform;

    [Header("Projectile Movement Variables")]
    public Vector3 moveDir = Vector3.right;
    public float moveSpeed = 0.5f;
    public Vector3 posLimitMin = Vector3.zero;
    public Vector3 posLimitMax = Vector3.zero;


    [Header("Scripts")]
    public PlayerManger playerManager;
    public PlayerState pstate;
    public GameManager gameManager;



    private void Start()
    {
        //변수 초기화
        isDestroyed = false;
    }


    private void Update()
    {
        if (!isDestroyed) 
        {
            //살아있는 판정일 때만 작동
            this.gameObject.transform.position = this.gameObject.transform.position + moveDir * moveSpeed * Time.deltaTime; //이동

            if (isPassedLimit())
            {
                //만약 화면 밖으로 나갔다면
                RegarndAsDestroyed();   //파괴처리
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayOneShotSound(enemy_hit_sound);
            //적과 충돌 시 작동

            if (!isDestroyed)
            {
                //파괴되지 않았을 때만 데미지 처리

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();   //충돌한 적의 데이터 가져오기

                int damage = CalculteDamage();      //데미지 계산
                enemy.status.EnemyHP -= damage;     //계산된 데미지 만큼 맞은 적 체력 감소
                enemy.getDamage();

                if (!pstate.Ball_Pierce_Power)
                {
                    //관통이 활성화 상태가 아니라면 적 접촉 시 파괴처리
                    RegarndAsDestroyed();
                }
            }
        }

    }


    //공격 시작 함수
    public void StartAttack()
    {
        this.gameObject.transform.position = InstantiateTransform.position; //공격이 날라가기 시작하는 위치로 이동
        player_attack_sound.Play();
        isDestroyed = false;    //움직이게 하기 위해 파괴 판정을 거짓으로
    }



    //데미지 값 받기
    private int CalculteDamage()
    {
        if (playerManager != null)
        {
            int atk = playerManager.GetTotalDamage();
            Debug.Log("-----------Total Damage: " + atk);
            return atk;
        }
        else 
        { 
            Debug.LogError("플레이어 스탯 참조 실패!"); 
            return 0;
        }
    }

    //파괴처리
    private void RegarndAsDestroyed()
    {
        isDestroyed = true; //움직이지 않도록 파괴 변수를 참으로
        this.gameObject.transform.position = returnTransform.position;  //초기 위치로 이동
        gameManager.NotifyProjectileDestroyed();    //진행을 위해 게임 시스템에 파괴되었다고 알림
    }


    //화면 밖으로 나갔는지 처리
    private bool isPassedLimit()
    {
        Vector3 temp = this.transform.position;

        if (temp.x >= posLimitMax.x ||
            temp.x <= posLimitMin.x ||
            temp.y >= posLimitMax.y ||
            temp.y <= posLimitMin.y)
        {
            return true;
        }
        else { return false; }
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
