using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityTitleText;
    [SerializeField] TextMeshProUGUI abilityDescriptionText;

    public void UpdateInfo(CrewAbilityScriptable ability)
    {
        abilityTitleText.text = ability.abilityName;
        abilityDescriptionText.text = ability.abilityDescription;
    }
}
