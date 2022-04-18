using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NameGenerator : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button doneButton;
    private string selectedName;

    public string GetName()
    {
        return selectedName;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnClickRandomMaleName();
    }

    public void OnClickRandomOldeMaleName()
    {
        selectedName = NVJOBNameGen.GiveAName(3);
        inputField.text = selectedName;
    }
    public void OnClickRandomMaleName()
    {
        selectedName = NVJOBNameGen.GiveAName(6);
        inputField.text = selectedName;
    }

    public void OnClickRandomOldeFemaleName()
    {
        selectedName = NVJOBNameGen.GiveAName(1);
        inputField.text = selectedName;
    }
    public void OnClickRandomFemaleName()
    {
        selectedName = NVJOBNameGen.GiveAName(5);
        inputField.text = selectedName;
    }

    public void OnClickDone()
    {
        selectedName = inputField.text;
        Debug.Log("The final name: " + selectedName);
    }


}
