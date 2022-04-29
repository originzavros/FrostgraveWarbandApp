using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInfoWindow : MonoBehaviour
{

    [SerializeField] DescriptionPanel descriptionPanel;
    [SerializeField] EquipmentList equipmentList;
    [SerializeField] StatCollapsablePanel statCollapsablePanel;
    [SerializeField] SoldierHeader soldierHeader;

    private SoldierScriptable storedSoldier;

    public void UpdatePanelInfo(SoldierScriptable soldier)
    {
        storedSoldier = soldier;
        descriptionPanel.UpdateDescription(soldier);
        equipmentList.UpdateEquipment(soldier);
        statCollapsablePanel.UpdateStats(soldier);
        soldierHeader.UpdateInfo(soldier);
    }

    public SoldierScriptable GetStoredSoldier()
    {
        return storedSoldier;
    }
    
}
