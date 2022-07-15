using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierTypeTextPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI typeTextField;

    public void UpdateDescription(RuntimeSoldierData soldier)
    {
        typeTextField.text = soldier.hiringName;
    }
}
