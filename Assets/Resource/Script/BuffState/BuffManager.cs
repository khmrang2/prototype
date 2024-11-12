using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffEffectState
{
    public int? Player_Damage;
    public float? Player_CriticalChance;
    public int? Player_HealthIncrease;
    public float? Player_DoubleUpChance;
    public int? Player_ShieldPower;
    public float? Ball_Size;
    public int? Ball_Count;
    public float? Ball_Elasticity;
    public int? Ball_PiercePower;
    public int? Ball_BallSplitCount;
}

[System.Serializable]
public class BuffEffect
{
    public int ID;
    public string Tooltip;
    public string ImagePath;
    public BuffEffectState buffEffect;
}

[System.Serializable]
public class BuffDataList
{
    public List<BuffEffect> buffs;
}

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel; // SelectPanel ������Ʈ
    [SerializeField] private TextAsset buffDataJSON; // JSON ����
    private Dictionary<int, BuffEffect> buffTable = new Dictionary<int, BuffEffect>(); // ID�� ������ �� �ִ� ���� ���̺�
    private bool isBuffSelected = false; // ������ ���õǾ����� ���θ� �����ϴ� �÷���
    private int selectedBuffId = -1; // ���õ� Buff ID

    private void Start()
    {
        LoadBuffDataFromJSON();
        selectPanel.SetActive(false); // ���� �� UI �г��� ��Ȱ��ȭ
    }

    // JSON ���Ͽ��� ��� ������ �ε��ϰ� ���̺� ����
    private void LoadBuffDataFromJSON()
    {
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonUtility.FromJson<BuffDataList>(jsonText);

        foreach (BuffEffect buff in dataList.buffs)
        {
            buffTable[buff.ID] = buff;
        }
    }

    // UI �г��� Ȱ��ȭ�ϰ�, ������ 3�� ������ ǥ��
    public void ShowBuffSelection()
    {
        isBuffSelected = false; // �ʱ�ȭ
        selectPanel.SetActive(true); // �г� Ȱ��ȭ
        ShowRandomBuffs();
    }

    // ������ 3���� ������ �����ϰ�, UI�� ǥ��
    private void ShowRandomBuffs()
    {
        List<BuffEffect> selectedBuffs = GetRandomBuffs(3);

        for (int i = 0; i < selectedBuffs.Count; i++)
        {
            BuffEffect buff = selectedBuffs[i];
            GameObject buffSlot = selectPanel.transform.GetChild(i).gameObject;

            Image buffImage = buffSlot.transform.Find("buffImage").GetComponent<Image>();
            TextMeshProUGUI buffText = buffSlot.transform.Find("buffToolTip").GetComponent<TextMeshProUGUI>();

            buffImage.sprite = LoadSpriteFromPath(buff.ImagePath);
            buffText.text = buff.Tooltip;

            int buffId = buff.ID;
            buffSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            buffSlot.GetComponent<Button>().onClick.AddListener(() => OnBuffSelected(buffId));
        }
    }

    // ������ n���� ������ �����ϴ� �Լ�
    private List<BuffEffect> GetRandomBuffs(int count)
    {
        List<BuffEffect> randomBuffs = new List<BuffEffect>(buffTable.Values);
        List<BuffEffect> selectedBuffs = new List<BuffEffect>();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, randomBuffs.Count);
            selectedBuffs.Add(randomBuffs[index]);
            randomBuffs.RemoveAt(index);
        }

        return selectedBuffs;
    }

    // ������ ���õǾ��� �� ȣ��Ǵ� �Լ�
    public void OnBuffSelected(int buffId)
    {
        selectedBuffId = buffId;
        isBuffSelected = true; // ���� �Ϸ�
        selectPanel.SetActive(false); // ���� �Ϸ� �� �г� ��Ȱ��ȭ
        Debug.Log("Selected Buff ID: " + buffId);
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
//private void ApplyBuffEffectToPlayer(BuffEffect buff)
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
//}