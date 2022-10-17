using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterKeywordButtonContainer : MonoBehaviour
{
    [SerializeField] GameObject monsterKeywordButton;
    [SerializeField] GameObject deleteButton;
    public GameObject GetMonsterKeywordButton()
    {
        return monsterKeywordButton;
    }
    public GameObject GetDeleteButton()
    {
        return deleteButton;
    }
}
