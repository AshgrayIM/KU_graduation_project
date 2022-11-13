using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            var inkList = GameObject.FindGameObjectsWithTag("Ink");
            for (int i = 0; i < inkList.Length; i++)
            {
                Destroy(inkList[i]);
            }
        }
    }
}
