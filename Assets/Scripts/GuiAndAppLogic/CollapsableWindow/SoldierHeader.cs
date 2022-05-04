using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soldierName;
    [SerializeField] TextMeshProUGUI soldierType;
    [SerializeField] TextMeshProUGUI soldierCost;

    public void UpdateInfo(RuntimeSoldierData soldier)
    {
        soldierName.text = soldier.hiringName;
        soldierType.text = soldier.soldierType;
        soldierCost.text = soldier.cost.ToString();
    }
}
