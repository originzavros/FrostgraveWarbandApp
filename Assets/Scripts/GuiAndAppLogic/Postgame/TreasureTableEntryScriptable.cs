using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//used with treasureTable to help determine treasure rewards
[CreateAssetMenu(fileName = "New TreasureEntry", menuName = "Assets/New TreasureEntry")]
public class TreasureTableEntryScriptable : ScriptableObject
{
    public int goldAmount = 0;
    public bool variableGold = false;
    public int variableGoldAmount  = 0;
    public List<MagicItemType> entryList = new List<MagicItemType>();
}
