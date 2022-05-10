using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    The basic equipment in the game.
    These could probably just be magic items sorted by BasicEquipment type, will have to refactor later, isn't hurting anything right now
*/
[CreateAssetMenu(fileName = "New Equipment", menuName = "Assets/New Equipment")]
public class EquipmentScriptable : ScriptableObject
{
    public string equipmentName;
    public string equipmentDescription;
    public FrostgraveBook bookEdition;

    

}



