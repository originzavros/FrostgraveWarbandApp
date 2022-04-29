using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHireWindow : MonoBehaviour
{
    [SerializeField] GameObject hireButton;
    [SerializeField] GameObject fireButton;
    [SerializeField] GameObject windowContents;

    public void SwitchToHireMode()
    {
        hireButton.SetActive(true);
        fireButton.SetActive(false);
    }
    public void SwitchToFireMode()
    {
        hireButton.SetActive(false);
        fireButton.SetActive(true);
    }

    public void SetHireEvent(UnityEngine.Events.UnityAction call)
    {
        hireButton.GetComponent<Button>().onClick.AddListener(call);
    }
    public void SetFireEvent(UnityEngine.Events.UnityAction call)
    {
        fireButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void AddItemToContents(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(windowContents.transform);
    }


}
