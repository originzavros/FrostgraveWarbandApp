using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EquipmentList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI equipmentText;

    public void UpdateEquipment(SoldierScriptable soldier)
    {
        string fill = "";
        foreach(var item in soldier.baseSoldierEquipment)
        {
            fill += item.equipmentName + ",";
        }
        equipmentText.text = fill;
    }
}
