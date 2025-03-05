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
using System.ComponentModel;


public class DataSettings   //����� ������ Ŭ����
{
    public int gold = 0;
    public int hp = 0;
    public int atk = 0;
    public int pinHp = 0;
    public int ballCount = 0;
    public int upgradeNum = 0;
}



public class DataControl : MonoBehaviour
{


    public DataSettings settings = new DataSettings();
    //�ҷ������� ����� �����͸� ���� Ŭ���� settings ����

    private string fileName = "savefile.dat";
    //Ŭ���忡 ����� ������ �̸� ����


    // ��ȣȭ Ű (16����Ʈ)
    private static readonly byte[] key = Encoding.UTF8.GetBytes("pX9rOL3mC3sEivmz");


    // ��ȣȭ IV (16����Ʈ)
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("KPlYJeefFqyw7WBe");



    //�ʱ� �����Ϳ��� �Էµ� �� �����͵��� Ű �̸�
    private static string PlayerHPName = "PlayerCharacter_HP";                 //�÷��̾� �ɸ����� ü���� Ű �̸�
    private static string PlayerATKName = "PlayerCharacter_ATK";               //�÷��̾� �ɸ����� ���ݷ��� Ű �̸�
    private static string PlayerBALLCOUNTName = "PlayerCharacter_BALLCOUNT";   //�÷��̾� �ɸ����� �� ���� Ű �̸�
    private static string PlayerPINHPName = "PlayerCharacter_PINHP";           //�÷��̾� �ɸ����� �� ü���� Ű �̸�

    private static string GoldName = "Gold";                                   //��ȭ �� ����� Ű �̸�

    private static string UpgradableNumName = "UpgradableNum";                 //���׷��̵� �ر� ������ Ű �̸�
    //private string EquipDataName;                                     //��� �ر� ������ Ű �̸�



    //�ʱ� �����Ϳ��� �Էµ� �� �����͵��� ��
    private static int PlayerHP = 100;                                         //�÷��̾� �ɸ����� �ʱ� ü�°�
    private static int PlayerATK = 5;                                          //�÷��̾� �ɸ����� �ʱ� ���ݷ°�
    private static int PlayerBALLCOUNT = 3;                                    //�÷��̾� �ɸ����� �ʱ� �� ���� ��
    private static int PlayerPINHP = 3;                                        //�÷��̾� �ɸ����� �ʱ� �� ü�°�

    private static int gold = 0;                                               //�ʱ� ��尪

    private static int UpgradableNum = 0;                                      //���׷��̵� �ر� ������ �ʱⰪ


    //���̺��� ���� ���θ� ��Ÿ���� bool ����
    private bool isSaveSuccess;



    #region gpgs Ŭ���� ������ ����


    public void SaveData() // �ܺο��� ���̺� ȣ��� �޼ҵ�
    {
        //���̺� ���� ���� �Ǵ��� ���� bool ����
        isSaveSuccess = false;

        OpenSaveGame();
    }







    private void OpenSaveGame() //���̺� �޼ҵ�
    {
        //player prefs�κ��� ������ ������ �޾ƿ���
        GetDataSettings();


        //gpgs�� �̱��� �ν��Ͻ��� ȣ��
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // ������ ����
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            onsavedGameOpend);
        //Ŭ���� ������ ���� ��û �޼ҵ�, ������ ���ϸ����� ����, �ɽ��� �ֽ��� �ƴ϶�� ��Ʈ��ũ�� ���� ������ ������, �浹�ø� ����� �ֽ� �����͸� ó��, �ݹ��Լ� onsavedGameOpend ����
    }


    private void onsavedGameOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        //������ ��û ���� ���ο� �ҷ����� �����͸� ���� �۵�

        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs �̱��� ȣ��

        if (status == SavedGameRequestStatus.Success)
        {
            //���̺� ��û�� �����ߴٸ�

            Debug.Log("save success!");
            //�α� ���

            var update = new SavedGameMetadataUpdate.Builder().Build();
            //���� �÷��� ���񽺿� ������ ���� ��Ÿ������ 

            //json
            var json = JsonUtility.ToJson(settings);
            //jsonUtility�� ���� �����Ϸ��� �����͸� ���ڿ��� ����

            byte[] data = Encoding.UTF8.GetBytes(json);
            //���ڿ��� ����� �����͸� ����Ʈ�� ��ȯ (UTF-8 ���)


            // ���� �Լ� ����
            saveGameClient.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.Log("Save No.....");

            //���̺� ���� ���� Ȯ�ο� ������ ���� �������� ����
            isSaveSuccess = false;
        }
    }

    // ���� Ȯ�� 
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //���� �޼ҵ� �۵� �� �ݹ��� ���� ����� �޼ҵ�

        if (status == SavedGameRequestStatus.Success)
        {
            // ����Ϸ�κ�
            Debug.Log("Save End");
            
            //���̺� ���� ���� Ȯ�ο� ������ ���� ������ ����
            isSaveSuccess = true;

        }
        else
        {
            Debug.Log("Save nonononononono...");
            
            //���̺� ���� ���� Ȯ�ο� ������ ���� �������� ����
            isSaveSuccess = false;
        }
    }






    #endregion








    #region gpgs Ŭ���� ������ �ε�


    public void LoadData()  //�ܺο��� �ε� ȣ��� �޼ҵ�
    {
        //���� ���� Ȯ�ο� bool ����
        isSaveSuccess = false;

        OpenLoadGame();
    }





    private void OpenLoadGame()
    {
        //gpgs�� �̱��� �ν��Ͻ��� ȣ��
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // ������ ����
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            LoadGameData);
        //Ŭ���� ������ ���� ��û �޼ҵ�, ������ ���ϸ����� ����, �ɽ��� �ֽ��� �ƴ϶�� ��Ʈ��ũ�� ���� ������ ������, �浹�ø� ����� �ֽ� �����͸� ó��, �ݹ��Լ� ����
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs �̱��� �ν��Ͻ� ȣ��
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs�� ���� ��û�� �����ߴٸ�
            Debug.Log("Load success");

            //gpgs�κ��� ����Ʈ �������� ����� �����͸� �޾ƿ��� �ݹ� �Լ� OnSavedGameDataRead ����
            savedGameClient.ReadBinaryData(data, OnSavedGameDataRead);

            //���������� Ȯ�ο� ������ true��
            isSaveSuccess = true;

        }
        else
        {
            Debug.Log("Load fail...");
            isSaveSuccess = false;
        }
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadData)
    {
        //�޾ƿ� ����Ʈ ������ �����͸� ���ڿ��� ��ȯ
        string data = System.Text.Encoding.UTF8.GetString(loadData);

        if (data == "")
        {
            //�޾ƿ� �����Ͱ� �����̶��
            Debug.Log("no saved data, saving initial data");

            //������ ����� �����Ͱ� ���ٴ� ���̰� �̴� �� �÷��̾ ���� ù �����̶�� ���̹Ƿ� �ʱⰪ ����
            SetInitialData();

            //�ش� ���� ����
            SaveData();
        }
        else
        {
            //�޾ƿ� �����Ͱ� ������ �ƴ϶��
            Debug.Log("Loading data");

            //JSON
            settings = JsonUtility.FromJson<DataSettings>(data);
            //logText.text = "Load complete";

            //gpgs�κ��� �ҷ����� �����ͷ� player prefs �ֽ�ȭ
            SetDataSettings();

        }
    }


    #endregion













    #region gpgs Ŭ���� ������ ����




    public void DeleteData()
    {
        DeleteGameData();
    }



    private void DeleteGameData()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs�� �̱��� �ν��Ͻ��� ȣ��

        // ������ ����
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            DeleteSaveGame);
        //Ŭ���� ������ ���� ��û �޼ҵ�, ������ ���ϸ����� ����, �ɽ��� �ֽ��� �ƴ϶�� ��Ʈ��ũ�� ���� ������ ������, �浹�ø� ����� �ֽ� �����͸� ó��, �ݹ��Լ� ����
    }


    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs�� �̱��� �ν��Ͻ� ȣ��
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs�� ���� ��û�� �޾Ƶ鿩���ٸ�

            //gpgs�� ����� ������ ����
            saveGameClient.Delete(data);

            Debug.Log("Delete Complete");
            //logText.text = "Delete complete";
        }
        else
        {
            Debug.Log("Delete fail");
            //logText.text = "Delete failed";
        }
    }


    #endregion










    #region PlayerPrefs�� ��ȣȭ ����



    //gpgs�κ��� �޾ƿ� data settings �����ͷ� player prefs�� ������ ���� 
    private void SetDataSettings()
    {
        SaveEncryptedDataToPrefs(GoldName, settings.gold.ToString());
        SaveEncryptedDataToPrefs(PlayerHPName, settings.hp.ToString());
        SaveEncryptedDataToPrefs(PlayerATKName, settings.atk.ToString());
        SaveEncryptedDataToPrefs(PlayerPINHPName, settings.pinHp.ToString());
        SaveEncryptedDataToPrefs(PlayerBALLCOUNTName, settings.ballCount.ToString());
        SaveEncryptedDataToPrefs(UpgradableNumName,settings.upgradeNum.ToString());

    }



    //player prefs�� ������ gpgs�� �����ϱ� ���� data settings�� ��������
    private void GetDataSettings()
    {
        settings.gold = int.Parse(LoadEncryptedDataFromPrefs(GoldName));
        settings.hp = int.Parse(LoadEncryptedDataFromPrefs(PlayerHPName));
        settings.atk = int.Parse(LoadEncryptedDataFromPrefs(PlayerATKName));
        settings.ballCount = int.Parse(LoadEncryptedDataFromPrefs(PlayerBALLCOUNTName));
        settings.pinHp = int.Parse(LoadEncryptedDataFromPrefs(PlayerPINHPName));
        settings.upgradeNum = int.Parse(LoadEncryptedDataFromPrefs(UpgradableNumName));
    }




    //player prefs�� ��ȣȭ���� �����͸� �����ϴ� �޼ҵ�, player prefs�� keyName�� Ű�� ��� ���ڿ� data�� ����


    //�� �Լ� ���!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static void SaveEncryptedDataToPrefs(string keyName, string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            //����� Ű���� �ʱ�ȭ ���Ͱ��� �޾ƿ�
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // ��ȣȭ Ű�� �ʱ�ȭ ���͸� �̿��Ͽ�, ��ȣȭ�� ������ encryptor ����
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encryptedData = null;

            // �Ϲ� �����͸� ��ȣȭ
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            // ��ȣȭ �����͸� ���ڿ��� ��ȯ�Ͽ� ���� 
            string encryptedString = Convert.ToBase64String(encryptedData);
            PlayerPrefs.SetString(keyName, encryptedString);
            PlayerPrefs.Save();

            //Debug.Log(encryptedString);
        }
    }




    //player prefs�� ��ȣȭ�Ǿ� ����� �����͸� �޾ƿ��� �޼ҵ�, ����� ����� keyName������ �����͸� ã�� ���ڿ� ������ ��ȯ

    //�� �Լ� ���!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static string LoadEncryptedDataFromPrefs(string keyName)
    {
        //player prefs�κ��� keyName���� ���� ��ȣȭ�� �����͸� encryptedString�� ����
        string encryptedString = PlayerPrefs.GetString(keyName);

        if (!string.IsNullOrEmpty(encryptedString))
        {
            //���� �޾ƿ� ���� ������� �ʴٸ�(= ���� �ִٸ�)
            using (Aes aesAlg = Aes.Create())
            {

                //Debug.Log(encryptedString);

                //����� Ű���� �ʱ�ȭ ���Ͱ��� �޾ƿ�
                aesAlg.Key = key;
                aesAlg.IV = iv;

                //��ȣȭ Ű�� �ʱ�ȭ ���͸� �̿��Ͽ� ��ȣȭ�� ������ decryptor ����
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // ������ ��ȣȭ
                byte[] encryptedData = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                // ��ȣȭ�� �����͸� �̿��Ͽ� ����� ������ ��ȯ
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        else
        {
            //�޾ƿ� ���� ����ִٸ�
            return null;
        }

    }



    #endregion







    #region �ʱ� ������ ���� ����

    
    //ȣ�� �� �÷��̾� �ɸ����� ����, ���׷��̵� �ر� �� ���� �ر� ������ �ʱⰪ ����
    public static void SetInitialData()
    {
        //�÷��̾� �ɸ����� �ʱ� ���� ����
        SaveEncryptedDataToPrefs(PlayerHPName, PlayerHP.ToString());
        SaveEncryptedDataToPrefs(PlayerATKName, PlayerATK.ToString());
        SaveEncryptedDataToPrefs(PlayerBALLCOUNTName, PlayerBALLCOUNT.ToString());
        SaveEncryptedDataToPrefs(PlayerPINHPName, PlayerPINHP.ToString());

        //�ʱ� ��尪 ����
        SaveEncryptedDataToPrefs(GoldName, gold.ToString());

        //���׷��̵� �ر� ������ �ʱⰪ ����
        SaveEncryptedDataToPrefs(UpgradableNumName, UpgradableNum.ToString());
    }




    #endregion





    #region ���� ���� Ȯ�� ����
    
    
    //���̺� �� �ε� ���� Ȯ�ο� �޼ҵ�
    public bool CheckSaveSuccess()
    {
        if (isSaveSuccess)
        {
            //���̺갡 ���������� �̷���� ���̺� ���� Ȯ�� ������ ���� ���̶��

            return true;    //true ��ȯ
        }
        else
        {
            //���̺갡 ���������� �̷������ ���� true �̿��� ���� ���� �Ǹ�

            return false;   //false ��ȯ
        }
    }


    #endregion


}
