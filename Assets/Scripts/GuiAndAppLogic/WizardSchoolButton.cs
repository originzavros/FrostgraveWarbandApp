using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WizardSchoolButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ButtonText;
    private WizardSchools containedWizardSchool;

    public WizardSchoolScriptable schoolScriptableReference;

    public void SetWizardSchool(WizardSchools incomingSchool)
    {
        containedWizardSchool = incomingSchool;
        ButtonText.text = containedWizardSchool.ToString();
    }

    public WizardSchools GetWizardSchool()
    {
        return containedWizardSchool;
    }


}
