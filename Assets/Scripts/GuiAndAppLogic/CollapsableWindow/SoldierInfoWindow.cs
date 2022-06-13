using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInfoWindow : MonoBehaviour
{

    [SerializeField] DescriptionPanel descriptionPanel;
    [SerializeField] EquipmentList equipmentList;
    [SerializeField] StatCollapsablePanel statCollapsablePanel;
    [SerializeField] SoldierHeader soldierHeader;

    private RuntimeSoldierData storedSoldier;

    public void UpdatePanelInfo(RuntimeSoldierData soldier, int hiringCostMod = 0)
    {
        storedSoldier = soldier;
        descriptionPanel.UpdateDescription(soldier);
        equipmentList.UpdateEquipment(soldier);
        statCollapsablePanel.UpdateStats(soldier);
        soldierHeader.UpdateInfo(soldier, hiringCostMod);
    }

    public RuntimeSoldierData GetStoredSoldier()
    {
        return storedSoldier;
    }
    
}
