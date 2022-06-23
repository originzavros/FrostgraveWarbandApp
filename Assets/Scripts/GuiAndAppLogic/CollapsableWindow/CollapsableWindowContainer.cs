using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapsableWindowContainer : MonoBehaviour
{
    [SerializeField] CollapsableWindow _collapsableWindow;

    [SerializeField] Button SwapButton;

    public CollapsableWindow GetCollapsableWindow()
    {
        return _collapsableWindow;
    }
    public Button GetSwapButton()
    {
        return SwapButton;
    }
}
