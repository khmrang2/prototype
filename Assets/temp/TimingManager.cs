using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{

    public List<GameObject> boxNoteList = new List<GameObject>();
    [SerializeField] private GameObject PlayerObj = null;
    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    [SerializeField] Vector3 boxNoteScale = new Vector3(1f, 1f, 1f);
    Vector2[] timingBoxs = null;
    PlayerControl playerCtr = null;

    // Start is called before the first frame update
    void Start()
    {
        timingBoxs = new Vector2[timingRect.Length];
        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width /2,
                            Center.localPosition.x + timingRect[i].rect.width /2);
        }

        playerCtr = PlayerObj.GetComponent<PlayerControl>();

        SetBoxNoteSizes(boxNoteScale);
    }
    void SetBoxNoteSizes(Vector3 scale)
    {
        foreach (GameObject boxNote in boxNoteList)
        {
            boxNote.transform.localScale = scale;
        }
    }

    public void CheckTiming()
    {
        for(int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for(int x= 0; x< timingBoxs.Length; x++)
            {

                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    playerCtr.movable = true;
                    return;
                }
            }
        }
        Debug.Log("Miss");
    }
}
