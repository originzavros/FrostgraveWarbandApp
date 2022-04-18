using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New School", menuName = "Assets/New School")]
public class WizardSchoolScriptable : ScriptableObject
{
    public WizardSchools primarySchool;

    public List<WizardSchools> alignedSchools;
    public List<WizardSchools> neutralSchools;
    public List<WizardSchools> enemySchools;

    public string GetSchoolName()
    {
        return primarySchool.ToString();
    }
}
