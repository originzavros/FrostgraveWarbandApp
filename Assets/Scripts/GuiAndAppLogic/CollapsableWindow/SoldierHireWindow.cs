using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    probably rename this to better represent actions, it isn't connected to anything for hiring directly
    also in future, can probably create a button prefab for adding more buttons to this since its meant to be flexible
    that way don't need to keep adding events per button
*/
public class SoldierHireWindow : MonoBehaviour
{
    [SerializeField] GameObject hireButton;
    [SerializeField] GameObject fireButton;
    [SerializeField] GameObject useButton;
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

    public void SwitchToUseMode()
    {
        hireButton.SetActive(false);
        fireButton.SetActive(false);
        useButton.SetActive(true);
    }

    public void SetHireEvent(UnityEngine.Events.UnityAction call)
    {
        hireButton.GetComponent<Button>().onClick.AddListener(call);
    }
    public void SetFireEvent(UnityEngine.Events.UnityAction call)
    {
        fireButton.GetComponent<Button>().onClick.AddListener(call);
    }
    public void SetUseEvent(UnityEngine.Events.UnityAction call)
    {
        useButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void AddItemToContents(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(windowContents.transform);
    }


}
