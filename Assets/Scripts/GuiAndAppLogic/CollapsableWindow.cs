using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapsableWindow : MonoBehaviour
{
    [SerializeField] GameObject windowBody;
    [SerializeField] GameObject ExpandButton;
    [SerializeField] GameObject CollapseButton;

    public void OnClickExpandButton()
    {
        ExpandButton.SetActive(false);
        CollapseButton.SetActive(true);
        windowBody.SetActive(true);
    }

    public void OnClickCollapseButton()
    {
        ExpandButton.SetActive(true);
        CollapseButton.SetActive(false);
        windowBody.SetActive(false);
    }

    //this should take in prefabs that are meant to fit under this window, so should have layoutelement with min height 50 or so
    public void AddItemToBody(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(windowBody.transform);
    }
}
