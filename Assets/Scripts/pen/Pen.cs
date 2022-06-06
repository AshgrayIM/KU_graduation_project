using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pen : MonoBehaviour
{
    public GameObject nib;
    public Material material;
    public TrailRenderer trailRenderer;
    public GameObject lineRenderer;
    private bool usingPen = false;
    private List<Vector3> vectorData;
    
    private int inkNo = 0;
    private string inkPrefix = "ink";

    public void Awake()
    {
        material.SetFloat("_Width", 0.005f);
        trailRenderer.material = material;
        
        var lr = lineRenderer.GetComponent<LineRenderer>();
        lr.material = material;
    }

    public void DrawEnter()
    {
        usingPen = true;
        trailRenderer.gameObject.SetActive(true);
    }

    public void DrawExit()
    {
        usingPen = false;
        trailRenderer.gameObject.SetActive(false);

        int positionCount = trailRenderer.positionCount;
        Vector3[] list = new Vector3[positionCount];
        trailRenderer.GetPositions(list);
        
        trailRenderer.Clear();
        lineRenderer = PhotonNetwork.Instantiate("lineRenderer", transform.position, transform.rotation);
        var lineObj = Instantiate(lineRenderer);
        lineObj.name = $"{inkPrefix} ({inkNo++})";
        var line = lineObj.GetComponent<LineRenderer>();
        line.positionCount = positionCount;
        line.SetPositions(list);
        line.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (usingPen)
        {
            vectorData.Add(nib.transform.position);
        }
    }
}
