using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarband 
{
    public string warbandName;
    public PlayerWizard warbandWizard;
    public List<SoldierScriptable> warbandSoldiers;

    public int warbandGold = 400;

    public List<EquipmentScriptable> warbandVault;
}
