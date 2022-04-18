using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandUIManager : MonoBehaviour
{
    [SerializeField] WarbandLister warbandLister;

    public void Init()
    {
        warbandLister.PopulateListerWithWarbands();
    }

    public void WarbandSelected(string name)
    {
        Debug.Log("Selected this warband:" + name);
    }
}
