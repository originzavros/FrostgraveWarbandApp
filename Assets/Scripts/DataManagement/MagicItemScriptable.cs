using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Assets/New Item")]
public class MagicItemScriptable : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemPurchasePrice;
    public int itemSalePrice;
    public FrostgraveBook itemBook;
    public MagicItemType itemType;

}

public enum MagicItemType{
    Weapon,
    Armor,
    LesserPotion,
    GreaterPotion,
    Artifact,
    Grimoire,
    Scroll

}
