using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Spell", menuName = "Assets/New Spell")]
public class SpellScriptable : SerializedScriptableObject
{
    public string Name;
    public int CastingNumber;
    public WizardSchools School;
    public string Restriction;
    public string Description;
    public FrostgraveBook bookEdition;

    public override string ToString()
    {
        string temp = "Name: " + Name + "\nCastingNumber: " + CastingNumber + "\nSchool: " + School + "\nRestriction: " + Restriction + "\nDescription: " + Description;
        return temp;
    }

}
