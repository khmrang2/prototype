using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class _UIStage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private Image stageThumbnail;

    public void setUpStageUI(string name, int level, Sprite thumb)
    {
        stageName.text = name + "\n" + level + "스테이지";
        stageThumbnail.sprite = thumb;
    }
}
