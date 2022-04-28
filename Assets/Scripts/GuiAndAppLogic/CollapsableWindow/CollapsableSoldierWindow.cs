using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableSoldierWindow : MonoBehaviour
{

    [SerializeField] DescriptionPanel descriptionPanel;
    [SerializeField] EquipmentList equipmentList;
    [SerializeField] StatCollapsablePanel statCollapsablePanel;
    [SerializeField] SoldierHeader soldierHeader;

    public void UpdatePanelInfo(SoldierScriptable soldier)
    {
        descriptionPanel.UpdateDescription(soldier);
        equipmentList.UpdateEquipment(soldier);
        statCollapsablePanel.UpdateStats(soldier);
        soldierHeader.UpdateInfo(soldier);
    }
    
}
