using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDiceContainer : MonoBehaviour
{
    public Button rollDiceButton;
    public GameObject spellButtonPrefab;
    public WizardRuntimeSpell wrs;
    public void Init(WizardRuntimeSpell _wrs){
        wrs = _wrs;
    }

    public void SetRollDiceEvent(UnityEngine.Events.UnityAction call)
    {
        rollDiceButton.GetComponent<Button>().onClick.AddListener(call);
    }
}
