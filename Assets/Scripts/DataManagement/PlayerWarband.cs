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
        warbandVault = new List<MagicItemRuntime>();
        warbandBonusSoldiers = new List<RuntimeSoldierData>();
    }
    public string warbandName;
    public PlayerWizard warbandWizard;
    public List<RuntimeSoldierData> warbandSoldiers;

    //meant to hold inn soldier, preserved soldier, or any extra's that are not part of the main warband
    public List<RuntimeSoldierData> warbandBonusSoldiers;

    public int warbandGold;
    public int warbandMaxSoldiers;

    public List<MagicItemRuntime> warbandVault;
}
