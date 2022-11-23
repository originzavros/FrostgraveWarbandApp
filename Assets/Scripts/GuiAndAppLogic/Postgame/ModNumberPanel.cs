using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModNumberPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI modNumberText;
    [SerializeField] TextMeshProUGUI modNumberNameText;

    private int modNumberValue;

    public void Init(string modNumberName, int initialValue = 0)
    {
        modNumberValue = initialValue;
        modNumberNameText.text = modNumberName;
        SetNumberText(modNumberValue.ToString());
    }

    public void OnClickPlusOne()
    {
        AdjustNumberText(1);
    }
    public void OnClickPlusFive()
    {
        AdjustNumberText(5);
    }

    public void OnClickMinusOne()
    {
        AdjustNumberText(-1);
    }
    public void OnClickMinusFive()
    {
        AdjustNumberText(-5);
    }

    public void AdjustNumberText(int adjust)
    {
        modNumberValue += adjust;
        if(modNumberValue < 0)
        {
            modNumberValue = 0;
        }
        SetNumberText(modNumberValue.ToString());
    }

    public void SetNumberText(string numberText)
    {
        modNumberText.text = numberText;
    }


    public int GetModNumberValue()
    {
        return modNumberValue;
    }

    public string GetPanelName()
    {
        return modNumberNameText.text;
    }
}
