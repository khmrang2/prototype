using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class BuffStruct
{
    public int ID;
    public string Name;
    public string Tooltip;
    public string ImagePath;
    public BaseState buffState;
}

[System.Serializable]
public class BuffDataList
{
    public List<BuffStruct> buffs;
}

public class BuffManager : MonoBehaviour
{
    public List<BuffSelectUI> buffUIs; // ���� ui.
    [SerializeField] public PlayerState playerState; 

    [SerializeField] private GameObject selectPanel; // SelectPanel ������Ʈ
    
    private TextAsset buffDataJSON; // JSON ����
    private Dictionary<int, BuffStruct> buffTable = new Dictionary<int, BuffStruct>(); // ID�� ������ �� �ִ� ���� ���̺�

    private List<BuffStruct> selectedBuffs; // ���õ� ������.
    private bool isBuffSelected = false; // ������ ���õǾ����� ���θ� �����ϴ� �÷���
    private int selectedBuffId = -1; // ���õ� Buff ID

    private BuffDataList dataList; // json���Ϸ� �о�� �������� ����Ʈ.

    private void Start()
    {
        LoadBuffDataFromJSON();
        selectPanel.SetActive(false); // ���� �� UI �г��� ��Ȱ��ȭ
    }

    // JSON ���Ͽ��� ��� ������ �ε��ϰ� ���̺� ����
    private void LoadBuffDataFromJSON()
    {
        buffDataJSON = Resources.Load<TextAsset>("Data/BuffData");
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonConvert.DeserializeObject<BuffDataList>(jsonText);

        // �����͸� �а� �� ���� �Ӽ��� ���
        foreach (BuffStruct buff in dataList.buffs)
        {
            //Debug.Log($"ID: {buff.ID}, Name: {buff.Name}");
            buffTable[buff.ID] = buff;
        }
    }

    // 1. UI �г��� Ȱ��ȭ�ϰ�, ������ 3�� ������ ǥ��
    public void ShowBuffSelection()
    {
        isBuffSelected = false; // �ʱ�ȭ
        selectPanel.SetActive(true); // �г� Ȱ��ȭ
        ShowRandomBuffs();
    }

    // 2. ������ 3���� ������ �����ϰ�, UI�� ǥ��
    private void ShowRandomBuffs()
    {
        // 3���� ���õ� ������ �������.
        List<BuffStruct> selectedBuffs = GetRandomBuffs(3);
        for (int i = 0; i < 3; i++)
        {
            // �� buffUI�� �ش� ���� ������ ����
            buffUIs[i].getBuffState(selectedBuffs[i]);

            // �ش� UI ������Ʈ�� Button ������Ʈ�� �ִٰ� �����ϰ�, Ŭ�� �̺�Ʈ�� ������ �߰�
            Button button = buffUIs[i].GetComponent<Button>();
            if (button != null)
            {
                // Ŭ���� �� ���� ����ϱ� ���� ���� ������ ����
                button.onClick.RemoveAllListeners();

                // ���������� �Ҵ��ؾ� �ùٸ� ����ID�� ĸó��
                int buffId = selectedBuffs[i].ID;
                button.onClick.AddListener(() => { OnBuffSelected(buffId); });
            }
            // ���� UI ����
        }
    }

    #region 2-1. ������ 3���� ������ �����ϴ� �Լ�.

    // 2-1. ������ n���� ������ �����ϴ� �Լ�
    private List<BuffStruct> GetRandomBuffs(int count)
    {
        selectedBuffs = new List<BuffStruct>();
        List<int> keys = new List<int>(buffTable.Keys); // ��� Ű ���� ����Ʈ�� ����

        // Fisher-Yates �˰������� Ű ����Ʈ ����
        for (int i = keys.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = keys[i];
            keys[i] = keys[j];
            keys[j] = temp;
        }

        // ���õ� ����Ʈ���� count����ŭ ���� ���� (����Ʈ ���̺��� count�� Ŭ �� �����Ƿ� ���� Ȯ��)
        for (int i = 0; i < count && i < keys.Count; i++)
        {
            int key = keys[i];
            if (buffTable.TryGetValue(key, out BuffStruct effect))
            {
                selectedBuffs.Add(effect);
            }
        }

        return selectedBuffs;
    }
    #endregion 

    // 3. ������ ���õǾ��� �� ȣ��Ǵ� �Լ�
    public void OnBuffSelected(int buffId)
    {
        selectedBuffId = buffId;
        BuffStruct selectedBuff;
        // ���� ���⿡�ٰ� ������ �����ϴ� �������� ����.
        if (buffTable.TryGetValue(buffId, out selectedBuff))
        {
            // ���õ� ������ buffState�� ApplyBuff�� ����
            applyBuff(selectedBuff.buffState);
        }
        else
        {
            Debug.LogWarning("Buff with ID " + buffId + " not found.");
        }
        // ���� ���ӸŴ����� ����������Ʈ�� �־�����.
        isBuffSelected = true; // ���� �Ϸ�
        selectPanel.SetActive(false); // ���� �Ϸ�� ���� â�� ��Ȱ��ȭ.
    }

    // ���� ���� �Ϸ� ���� Ȯ�� �޼���
    public bool IsBuffSelected()
    {
        return isBuffSelected;
    }

    // ���õ� Buff ID ��ȯ
    public int GetSelectedBuffId()
    {
        return selectedBuffId;
    }

    private void applyBuff(BaseState effect)
    {
        playerState.AddState(effect);
    }

    public void PrintBuffSumState()
    {
        //Debug.Log("BuffSumState Fields:");

        //// BuffSumState�� ��� public �ν��Ͻ� �ʵ带 ��ȸ
        //foreach (FieldInfo field in typeof(BaseState).GetFields(BindingFlags.Public | BindingFlags.Instance))
        //{
        //    // �ʵ� �̸��� �ش� ���� ������ ���
        //    var value = field.GetValue(selectedBuffI);
        //    Debug.Log($"{field.Name}: {value}");
        //}
    }

    // �̹��� ��ηκ��� ��������Ʈ �ε� (Resources ���� ��� ��) -? �� �ȵ���..����
    private Sprite LoadSpriteFromPath(string path)
    {
        //Debug.Log(path);

        Sprite loadedSprite = Resources.Load<Sprite>("Image/BuffImage/swordicon");

        if (loadedSprite != null)
        {
            Debug.Log($"Loaded sprite: {loadedSprite.name}, Size: {loadedSprite.rect.size}");
        }
        else
        {
            Debug.LogError("Failed to load sprite at path: " + "Image/BuffImage/swordicon");
        }

        return loadedSprite;
    }
    //private void ApplyBuffEffectToPlayer(BuffState buff)
    //{
    //    if (buff.Player_Damage.HasValue)
    //        buffState.Player_Damage += buff.Player_Damage.Value;

    //    if (buff.Player_CriticalChance.HasValue)
    //        buffState.Player_CriticalChance += buff.Player_CriticalChance.Value;

    //    if (buff.Player_HealthIncrease.HasValue)
    //        buffState.Player_HealthIncrease += buff.Player_HealthIncrease.Value;

    //    if (buff.Player_DoubleUpChance.HasValue)
    //        buffState.Player_DoubleUpChance += buff.Player_DoubleUpChance.Value;

    //    if (buff.Player_ShieldPower.HasValue)
    //        buffState.Player_ShieldPower += buff.Player_ShieldPower.Value;

    //    if (buff.Ball_Size.HasValue)
    //        buffState.Ball_Size += buff.Ball_Size.Value;

    //    if (buff.Ball_Count.HasValue)
    //        buffState.Ball_Count += buff.Ball_Count.Value;

    //    if (buff.Ball_Elasticity.HasValue)
    //        buffState.Ball_Elasticity += buff.Ball_Elasticity.Value;

    //    if (buff.Ball_PiercePower.HasValue)
    //        buffState.Ball_PiercePower += buff.Ball_PiercePower.Value;

    //    if (buff.Ball_BallSplitCount.HasValue)
    //        buffState.Ball_BallSplitCount += buff.Ball_BallSplitCount.Value;
    //}

}

//// ���� �Ŵ���
//// 1. json ������ load�Ͽ� buffeffect ���̺��� ������.
//// 2. ui�� ������ �����ְ� �����ϴ� ���
//// 3. 
//public GameManager gameManager; // ���� �Ŵ��� ����

//PlayerState playerDefaultState = null;

//[SerializeField] private BuffState buffState; // buffState ����
//private List<BuffEffect> allBuffs = new List<BuffEffect>(); // ��� Ȱ��ȭ�� ����
//private List<BuffEffect> newBuffs = new List<BuffEffect>(); // ���� �߰��� ����

//private void Start()
//{
//    // �÷��̾� ������Ʈ�� ���� ���� �ؼ� ������.
//    // => json���� load�ؼ� �÷��̾��� ������ �������� �������� �ٲ� �� ����.
//    playerDefaultState = new PlayerState();
//}

//// �� ������ �߰��ϰ� newBuffs�� ����
//public void addBuff(BuffEffect buff)
//{
//    newBuffs.Add(buff);
//}

//// �� ������ ���ؼ� ������Ʈ ���־� buffState�� ������Ʈ.
//public BuffState updateBuffState()
//{
//    foreach (BuffEffect buff in newBuffs)
//    {
//        ApplyBuffEffectToPlayer(buff);
//        allBuffs.Add(buff); // ������ ������ allBuffs�� �̵�
//    }

//    newBuffs.Clear(); // newBuffs �ʱ�ȭ
//    return buffState;
//}

//// ���ڷ� ���� ������ ����ϴ� �޼ҵ�
//

//public BuffState getBuffState()
//{
//    return this.buffState;
//}
////----------------------------------------------------------------------------------
////----------------------------------------------------------------------------------
////                                  ���� ui ���� ����
////----------------------------------------------------------------------------------
////----------------------------------------------------------------------------------
//[SerializeField] private GameObject buffUIPrefab; // UI ������
//[SerializeField] private Transform uiParent; // UI�� ��ġ�� �θ� ��ü
//private List<GameObject> uiPool = new List<GameObject>(); // UI ������Ʈ Ǯ�� ����Ʈ

////private void Start()
////{
////    InitializeUIPool(3); // ���÷� 5���� UI�� Ǯ���ص�
////}

//// UI ������Ʈ Ǯ�� �ʱ�ȭ
//private void InitializeUIPool(int count)
//{
//    for (int i = 0; i < count; i++)
//    {
//        GameObject ui = Instantiate(buffUIPrefab, uiParent);
//        ui.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
//        uiPool.Add(ui);
//    }
//}

//// UI�� Ȱ��ȭ�ϰ� ������ ������Ʈ
//public void ShowBuffUI(BuffEffect buffEffect)
//{
//    GameObject ui = GetAvailableUI();
//    if (ui != null)
//    {
//        ui.SetActive(true);
//        UpdateBuffUI(ui, buffEffect); // UI ��Ҹ� ���� �����ͷ� ������Ʈ
//    }
//}

//// ��� ������ UI�� ��ȯ
//private GameObject GetAvailableUI()
//{
//    foreach (var ui in uiPool)
//    {
//        if (!ui.activeInHierarchy)
//            return ui;
//    }
//    return null; // ��� ������ UI�� ������ null ��ȯ
//}

//// UI ������Ʈ (���� �̹���, ���� �� ����)
//private void UpdateBuffUI(GameObject ui, BuffEffect buffEffect)
//{
//    Image buffImage = ui.transform.Find("BuffImage").GetComponent<Image>();
//    Text buffTooltip = ui.transform.Find("BuffTooltip").GetComponent<Text>();

//    // ���� �̹����� ������ ����
//    buffImage.sprite = LoadSpriteFromPath(buffEffect.ImagePath);
//    buffTooltip.text = buffEffect.Tooltip;
//}

//// �̹��� ��ηκ��� ��������Ʈ �ε� (Resources ���� ��� ��)
//private Sprite LoadSpriteFromPath(string path)
//{
//    return Resources.Load<Sprite>(path);
//}

//// ��� UI�� ���� (�ʿ�� ��ü UI �ʱ�ȭ)
//public void HideAllUI()
//{
//    foreach (var ui in uiPool)
//    {
//        ui.SetActive(false);
//    }

// �� ���⼭ null ref error?
//Debug.Log(selectedBuffs[buffId].buffState.Player_Damage);
//ApplyBuff(selectedBuffs[buffId].buffState);
//}