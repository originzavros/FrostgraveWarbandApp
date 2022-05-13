using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PostgameSoldierButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;

    private RuntimeSoldierData soldierReference;

    public void Init(RuntimeSoldierData rsd)
    {
        soldierReference = rsd;
    }

    public void UpdateText(string text)
    {
        buttonText.text = text;
    }

    public RuntimeSoldierData GetStoredSoldier()
    {
        return soldierReference;
    }
    public void SetButtonEvent(UnityEngine.Events.UnityAction call)
    {
        this.GetComponent<Button>().onClick.AddListener(call);
    }

}
