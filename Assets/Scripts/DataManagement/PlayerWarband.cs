using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerWarband
{
    public string warbandName;
    public PlayerWizard warbandWizard;
    public List<SoldierScriptable> warbandSoldiers = new List<SoldierScriptable>();
    public SoldierScriptable[] saveSoldierArray;

    public int warbandGold = 400;
    public int warbandMaxSoldiers = 9;

    public List<MagicItemScriptable> warbandVault = new List<MagicItemScriptable>();
}
