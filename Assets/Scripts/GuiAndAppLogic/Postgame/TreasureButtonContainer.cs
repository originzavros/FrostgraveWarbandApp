using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureButtonContainer : MonoBehaviour
{
    [SerializeField] Button treasureButton;
    [SerializeField] Button deleteButton;
    [SerializeField] TextMeshProUGUI treasureButtonText;

    public void SetTreasureButtonText(string input)
    {
        treasureButtonText.text = input;
    }
    public string GetTreasureButtonText()
    {
        return treasureButtonText.text;
    }
    public Button GetTreasureButton()
    {
        return treasureButton;
    }
    public Button GetDeleteButton()
    {
        return deleteButton;
    }

    public void OnClickDeletButton()
    {
        Destroy(this.gameObject);
    }
}
