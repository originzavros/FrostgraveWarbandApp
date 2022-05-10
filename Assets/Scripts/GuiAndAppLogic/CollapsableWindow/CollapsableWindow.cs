using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapsableWindow : MonoBehaviour
{
    [SerializeField] GameObject windowBody;
    [SerializeField] GameObject ExpandButton;
    [SerializeField] GameObject CollapseButton;
    [SerializeField] GameObject expandImage;
    [SerializeField] GameObject collapseImage;

    public void OnClickExpandButton()
    {
        ExpandButton.SetActive(false);
        CollapseButton.SetActive(true);
        windowBody.SetActive(true);
        expandImage.SetActive(false);
        collapseImage.SetActive(true);
        // windowBody.GetComponent<LayoutElement>().minHeight = 300;
        // this.gameObject.GetComponentInParent<VerticalLayoutGroup>().spacing += .01f;
        // Canvas.ForceUpdateCanvases();
    }

    public void OnClickCollapseButton()
    {
        ExpandButton.SetActive(true);
        CollapseButton.SetActive(false);
        windowBody.SetActive(false);
        expandImage.SetActive(true);
        collapseImage.SetActive(false);
        // this.gameObject.GetComponentInParent<VerticalLayoutGroup>().spacing -= .01f;
        // Canvas.ForceUpdateCanvases();
    }

    //this should take in prefabs that are meant to fit under this window, so should have layoutelement with min height 50 or so
    public void AddItemToBody(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(windowBody.transform);
    }

    public void SetBodyPermaActive()
    {
        expandImage.SetActive(false);
        ExpandButton.SetActive(false);
        CollapseButton.SetActive(false);
        windowBody.SetActive(true);
    }
}
