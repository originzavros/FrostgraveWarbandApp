using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellTextPopup : MonoBehaviour
{
    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI castingNumberText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI schoolValueText;
    public TextMeshProUGUI rangetypeValueText;

    public void UpdateInfo(SpellScriptable ss)
    {
        spellNameText.text = ss.name;
        castingNumberText.text = ss.CastingNumber.ToString();
        descriptionText.text = ss.Description;
        schoolValueText.text = ss.School.ToString();
        rangetypeValueText.text = ss.Restriction.ToString();
    }

    public void UpdateRuntimeInfo(WizardRuntimeSpell ss)
    {
        spellNameText.text = ss.referenceSpell.Name;
        castingNumberText.text = (ss.referenceSpell.CastingNumber + ss.GetAllMods()).ToString();
        descriptionText.text = ss.referenceSpell.Description;
        schoolValueText.text = ss.referenceSpell.School.ToString();
        rangetypeValueText.text = ss.referenceSpell.Restriction.ToString();
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
