using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandContainer : MonoBehaviour
{
    [SerializeField] GameObject warbandButton;
    [SerializeField] GameObject deleteButton;

    public GameObject GetWarbandButton()
    {
        return warbandButton;
    }
    public GameObject GetDeleteButton()
    {
        return deleteButton;
    }
}
