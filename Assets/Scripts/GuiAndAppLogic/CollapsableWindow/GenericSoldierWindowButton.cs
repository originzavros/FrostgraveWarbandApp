using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class GenericSoldierWindowButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI basicButtonText;
    [SerializeField] Button basicButton;

    public void Init(string _text)
    {
        basicButtonText.text = _text;
    }

    public void SetPopupEvent(UnityEngine.Events.UnityAction call)
    {
        basicButton.onClick.AddListener(call);
    }

    public void SetColor(Color _newColor)
    {
        this.gameObject.GetComponent<Image>().color = _newColor;
    }
}
