using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using System;


public class DataSettings   //저장될 데이터 클래스
{
    public int gold = 0;
}



public class DataControl : MonoBehaviour
{

    public TextMeshProUGUI logText;
    public TextMeshProUGUI goldText;

    public DataSettings settings = new DataSettings();
    //불러와지고 저장될 데이터를 담을 클래스 settings 선언

    private string fileName = "savefile.dat";
    //클라우드에 저장될 파일의 이름 설정


    // 암호화 키 (16바이트)
    private static readonly byte[] key = Encoding.UTF8.GetBytes("pX9rOL3mC3sEivmz");


    // 암호화 IV (16바이트)
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("KPlYJeefFqyw7WBe");










    #region gpgs 클라우드 데이터 저장


    public void SaveData() // 외부에서 세이브 호출용 메소드
    {
        OpenSaveGame();
    }







    private void OpenSaveGame() //세이브 메소드
    {
        //player prefs로부터 저장할 데이터 받아오기
        GetDataSettings();


        //gpgs의 싱글톤 인스턴스를 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            onsavedGameOpend);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 onsavedGameOpend 실행
    }


    private void onsavedGameOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        //데이터 요청 성공 여부와 불러와진 데이터를 통해 작동

        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs 싱글톤 호출

        if (status == SavedGameRequestStatus.Success)
        {
            //세이브 요청에 성공했다면

            Debug.Log("save success!");
            //로그 출력

            var update = new SavedGameMetadataUpdate.Builder().Build();
            //구글 플레이 서비스에 저장을 위한 메타데이터 

            //json
            var json = JsonUtility.ToJson(settings);
            //jsonUtility를 통해 저장하려는 데이터를 문자열로 변경

            byte[] data = Encoding.UTF8.GetBytes(json);
            //문자열로 변경된 데이터를 바이트로 변환 (UTF-8 사용)


            // 저장 함수 실행
            saveGameClient.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.Log("Save No.....");
        }
    }

    // 저장 확인 
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //저장 메소드 작동 시 콜백을 통해 실행될 메소드

        if (status == SavedGameRequestStatus.Success)
        {
            // 저장완료부분
            Debug.Log("Save End");
            logText.text = "Save complete";
        }
        else
        {
            Debug.Log("Save nonononononono...");
            logText.text = "Save failed";
        }
    }


    #endregion








    #region gpgs 클라우드 데이터 로드


    public void LoadData()  //외부에서 로드 호출용 메소드
    {
        OpenLoadGame();
    }





    private void OpenLoadGame()
    {
        //gpgs의 싱글톤 인스턴스를 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            LoadGameData);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 실행
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs 싱글톤 인스턴스 호출
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs에 보낸 요청이 성공했다면
            Debug.Log("Load success");

            //gpgs로부터 바이트 형식으로 저장된 데이터를 받아오고 콜백 함수 OnSavedGameDataRead 실행
            savedGameClient.ReadBinaryData(data, OnSavedGameDataRead);
        }
        else
        {
            Debug.Log("Load fail...");
        }
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadData)
    {
        //받아온 바이트 형태의 데이터를 문자열로 변환
        string data = System.Text.Encoding.UTF8.GetString(loadData);

        if (data == "")
        {
            //받아온 데이터가 공백이라면
            Debug.Log("no saved data, saving initial data");

            //기존에 저장된 데이터가 없다는 뜻이므로 현제의 데이터를 저장
            SaveData();
        }
        else
        {
            //받아온 데이터가 공백이 아니라면
            Debug.Log("Loading data");

            //JSON
            settings = JsonUtility.FromJson<DataSettings>(data);
            logText.text = "Load complete";

            //gpgs로부터 불러와진 데이터로 player prefs 최신화
            SetDataSettings();

        }
    }


    #endregion













    #region gpgs 클라우드 데이터 제거




    public void DeleteData()
    {
        DeleteGameData();
    }



    private void DeleteGameData()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs의 싱글톤 인스턴스를 호출

        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            DeleteSaveGame);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 실행
    }


    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs의 싱글톤 인스턴스 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs에 보낸 요청이 받아들여졌다면

            //gpgs에 저장된 데이터 삭제
            saveGameClient.Delete(data);

            Debug.Log("Delete Complete");
            logText.text = "Delete complete";
        }
        else
        {
            Debug.Log("Delete fail");
            logText.text = "Delete failed";
        }
    }


    #endregion










    #region PlayerPrefs와 암호화 관련



    //gpgs로부터 받아온 data settings 데이터로 player prefs의 값들을 변경 
    private void SetDataSettings()
    {
        SaveEncryptedDataToPrefs("Gold", settings.gold.ToString());
        //SaveEncryptedDataToPrefs("저장변수명", settings.ToString.example);
        //SaveEncryptedDataToPrefs("저장변수명", settings.ToString.example);

    }



    //player prefs의 값들을 gpgs에 저장하기 위해 data settings로 가져오기
    private void GetDataSettings()
    {
        settings.gold = int.Parse(LoadEncryptedDataFromPrefs("Gold"));
        //LoadEncryptedDataFromPrefs("저장변수명");
        //LoadEncryptedDataFromPrefs("저장변수명");
    }




    //player prefs에 암호화시켜 데이터를 저장하는 메소드, player prefs에 keyName을 키로 삼아 문자열 data을 저장


    //이 함수 사용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static void SaveEncryptedDataToPrefs(string keyName, string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            //저장된 키값과 초기화 벡터값을 받아옴
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // 암호화 키와 초기화 벡터를 이용하여, 암호화를 진행할 encryptor 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encryptedData = null;

            // 일반 데이터를 암호화
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            // 암호화 데이터를 문자열로 변환하여 저장 
            string encryptedString = Convert.ToBase64String(encryptedData);
            PlayerPrefs.SetString(keyName, encryptedString);
            PlayerPrefs.Save();

            //Debug.Log(encryptedString);
        }
    }




    //player prefs에 암호화되어 저장된 데이터를 받아오는 메소드, 저장시 사용한 keyName값으로 데이터를 찾아 문자열 값으로 반환

    //이 함수 사용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static string LoadEncryptedDataFromPrefs(string keyName)
    {
        //player prefs로부터 keyName값을 통해 암호화된 데이터를 encryptedString에 저장
        string encryptedString = PlayerPrefs.GetString(keyName);

        if (!string.IsNullOrEmpty(encryptedString))
        {
            //만일 받아온 값이 비어있지 않다면(= 값이 있다면)
            using (Aes aesAlg = Aes.Create())
            {

                //Debug.Log(encryptedString);

                //저장된 키값과 초기화 벡터값을 받아옴
                aesAlg.Key = key;
                aesAlg.IV = iv;

                //암호화 키와 초기화 벡터를 이용하여 복호화를 진행할 decryptor 생성
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 데이터 복호화
                byte[] encryptedData = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                // 복호화된 데이터를 이용하여 저장된 데이터 반환
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        else
        {
            //받아온 값이 비어있다면
            return null;
        }

    }



    #endregion


    private void Start()
    {
        goldText.text = "0";
    }


    private void Update()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            goldText.text = LoadEncryptedDataFromPrefs("Gold");
        }
        else
        {
            SaveEncryptedDataToPrefs("Gold", goldText.text);
            goldText.text = LoadEncryptedDataFromPrefs("Gold");
        }
    }



    public void AddGold()
    {
        int tempGold = int.Parse(goldText.text) + 100;
        SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
    }


    public void MinusGold()
    {
        int tempGold = int.Parse(goldText.text) - 100;
        SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
    }





}
