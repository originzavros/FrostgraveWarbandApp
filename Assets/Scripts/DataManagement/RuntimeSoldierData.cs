using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for saving loading easily with ES3 as scriptables can't be saved by instance
public class RuntimeSoldierData
{
    public void Init(SoldierScriptable ss)
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
        monsterKeywordList = ss.monsterKeywordList;
    }

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
        monsterKeywordList = ss.monsterKeywordList;
    }

    //ideally these would all be properties as stats could be affected by abilities/spells, but not going that far with rules enforcement
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
    public List<MagicItemRuntime> soldierInventory = new List<MagicItemRuntime>();
    public List<InjuryScriptable> soldierPermanentInjuries = new List<InjuryScriptable>();
    public List<MonsterKeywordScriptable> monsterKeywordList = new List<MonsterKeywordScriptable>();
}
