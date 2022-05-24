using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Injury", menuName = "Assets/New Injury")]
public class InjuryScriptable : ScriptableObject
{
   public string injuryName;
   public string injuryStat;
   public int injuryMod;
   public int injuryMax;
   public string injuryDescription;

}
