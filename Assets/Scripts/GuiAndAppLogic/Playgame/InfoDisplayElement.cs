using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplayElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayText;

    public void UpdateText(string textchange)
    {
        displayText.text = textchange;
    }
}
