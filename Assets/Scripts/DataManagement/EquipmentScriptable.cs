using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Slightly misleading, but this holds both equipment and items
    They aren't really functionally different in the game in most cases
*/
[CreateAssetMenu(fileName = "New Equipment", menuName = "Assets/New Equipment")]
public class EquipmentScriptable : ScriptableObject
{
    public string equipmentName;
    public string equipmentDescription;
    public FrostgraveBook bookEdition;

    

}



