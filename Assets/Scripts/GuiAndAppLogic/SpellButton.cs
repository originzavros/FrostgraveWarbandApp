using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellButton : MonoBehaviour
{
    public TextMeshProUGUI SpellNameText;
    public TextMeshProUGUI CastingNumberText;

    public SpellScriptable referenceScriptable;

    public void LoadSpellInfo(SpellScriptable _spell)
    {
        referenceScriptable = _spell;
        SpellNameText.text = referenceScriptable.Name;
        CastingNumberText.text = referenceScriptable.CastingNumber.ToString();
    }
}
