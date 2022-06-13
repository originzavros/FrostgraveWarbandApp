using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soldierName;
    [SerializeField] TextMeshProUGUI soldierType;
    [SerializeField] TextMeshProUGUI soldierCost;

    public void UpdateInfo(RuntimeSoldierData soldier, int hiringCostMod = 0)
    {
        soldierName.text = soldier.hiringName;
        soldierType.text = soldier.soldierType;
        int moddedCost = soldier.cost + hiringCostMod;
        if(moddedCost < 0){moddedCost = 0;}
        soldierCost.text = moddedCost.ToString();
    }
}
