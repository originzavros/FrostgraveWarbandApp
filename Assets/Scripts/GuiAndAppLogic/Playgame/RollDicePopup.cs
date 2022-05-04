using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollDicePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rollDiceValue;
    [SerializeField] TextMeshProUGUI rollShootValue;
    [SerializeField] TextMeshProUGUI rollFightValue;
    [SerializeField] TextMeshProUGUI rollWillValue;

    private RuntimeSoldierData rollingSoldier;
    private int currentRoll = 0;
    public void Init(RuntimeSoldierData rsd)
    {
        rollingSoldier = rsd;
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
        rollShootValue.text = (roll + rollingSoldier.shoot).ToString();
        rollFightValue.text = (roll + rollingSoldier.fight).ToString();
        rollWillValue.text = (roll + rollingSoldier.will).ToString();

    }
}
