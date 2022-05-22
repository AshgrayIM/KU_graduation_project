using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public GameObject nip;
    public void Draw()
    {
        Debug.LogError(nip.transform.position);
    }
}
