using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public GameObject nip;
    private bool usingPen = false;
    public void DrawEnter()
    {
        usingPen = true;
    }

    public void DrawExit()
    {
        usingPen = false;
    }

    public void Update()
    {
        if (usingPen)
        {
            Debug.LogError(nip.transform.position);
        }
    }
}
