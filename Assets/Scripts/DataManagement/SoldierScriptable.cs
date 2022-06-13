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
    public void Init(RuntimeSoldierData ss)
    {
        soldierName = ss.soldierName;
        inventoryLimit = ss.inventoryLimit;
        move = ss.move;
        fight = ss.fight;
        shoot = ss.shoot;
        armor = ss.armor;
        will = ss.will;
        health = ss.health;
        cost = ss.cost;
        hiringName = ss.hiringName;
        soldierType = ss.soldierType;
        isHired = ss.isHired;
        description = ss.description;
        bookEdition = ss.bookEdition;
        baseSoldierEquipment = ss.baseSoldierEquipment;
        // soldierInventory = ss.soldierInventory;
        status = ss.status;
    }
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
    public SoldierStatus status = SoldierStatus.ready;
    public FrostgraveBook bookEdition;
    public List<EquipmentScriptable> baseSoldierEquipment = new List<EquipmentScriptable>();
    public List<MagicItemScriptable> soldierInventory = new List<MagicItemScriptable>();
}