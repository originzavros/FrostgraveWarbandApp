using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerWarband
{

    public void Init()
    {
        warbandWizard = new PlayerWizard();
        warbandGold = 400;
        warbandMaxSoldiers = 9;
        warbandSoldiers = new List<RuntimeSoldierData>();
        warbandVault = new List<MagicItemScriptable>();
    }
    public string warbandName;
    public PlayerWizard warbandWizard;
    public List<RuntimeSoldierData> warbandSoldiers;

    public int warbandGold;
    public int warbandMaxSoldiers;

    public List<MagicItemScriptable> warbandVault;
}
