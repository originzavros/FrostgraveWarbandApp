using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//mainly for determining what types of treasure to generate at random
[CreateAssetMenu(fileName = "New TreasureTable", menuName = "Assets/New TreasureTable")]
public class TreasureTableScriptable : ScriptableObject
{
    public string tableName = "new table";

    public FrostgraveBook expansion = FrostgraveBook.Core;

    public List<TreasureTableEntryScriptable> treasureList = new List<TreasureTableEntryScriptable>();
}

