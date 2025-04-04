using System;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject buff,debuff;
    
    private float timeCounter=0;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter > 1.2f)
            {
                timeCounter = 0;
                buff.SetActive(false);
            }
        }
        if (debuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter > 1.2f)
            {
                timeCounter = 0;
                debuff.SetActive(false);
            }
        }
    }
}
