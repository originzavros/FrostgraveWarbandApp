using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is a fix for the weird scaling issues for multi layered layout groups
public class AutoScaler : MonoBehaviour
{
    private float scaleValue = 0;
    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x > 1)
        {
            scaleValue = 1 - this.transform.localScale.x;
            this.transform.localScale += new Vector3(scaleValue, scaleValue, 0);
        }
    }
}
