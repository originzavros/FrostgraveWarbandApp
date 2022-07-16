using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollSpellPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rollDiceValue;
    [SerializeField] TextMeshProUGUI spellTNValue;
    [SerializeField] TextMeshProUGUI apprenticeRollValue;
    [SerializeField] TextMeshProUGUI rollWillValue;
    [SerializeField] TextMeshProUGUI spellName;

    public WizardRuntimeSpell wrs;
    private int currentRoll = 0;

    public void Init(WizardRuntimeSpell _wrs)
    {
        wrs = _wrs;
        spellName.text = wrs.referenceSpell.Name;
        Roll();
    }

    public void Roll()
    {
        currentRoll = Random.Range(1, 20);
        UpdateAllFields(currentRoll);
    }

    public void UpdateAllFields(int roll)
    {
        rollDiceValue.text = roll.ToString();
        spellTNValue.text = wrs.GetFullModedCastingNumber().ToString();
        apprenticeRollValue.text = (roll - 2).ToString();

    }
}
