using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Soldier", menuName = "Assets/New Soldier")]
public class SoldierScriptable : ScriptableObject
{
    /*
        Hold predefined soldier data like stats and starting equipment.
        wizards and monsters will use this as their base with their own extensions
    */

    public string soldierName;
    public int inventoryLimit = 1;
    public int move;
    public int fight;
    public int shoot;
    public int armor;
    public int will;
    public int health;
    public int cost;
    public string hiringName;
    public string soldierType;
    public bool isHired = false; //for the warband builder
    public string description;
    public FrostgraveBook bookEdition;
    public List<EquipmentScriptable> baseSoldierEquipment;
    public List<MagicItemScriptable> soldierInventory;




}
