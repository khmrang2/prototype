using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GUIBuildScript : MonoBehaviour
{

    [SerializeField] private DynamicGridLayout ui1;
    [SerializeField] private DynamicGridLayout ui2;
    public void build()
    {
        ui1.UpdateGridLayout();
        ui2.UpdateGridLayout();
    }
}
