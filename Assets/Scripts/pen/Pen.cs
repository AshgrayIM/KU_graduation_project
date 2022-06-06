using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pen : MonoBehaviour
{
    // public GameObject nib;
    public Material material;
    public TrailRenderer trailRenderer;
    public GameObject lineRenderer;
    // private bool usingPen = false;
    // private List<Vector3> vectorData;
    private PhotonView photonView;
    
    private int inkNo = 0;
    private string inkPrefix = "ink";

    public void Awake()
    {
        material.SetFloat("_Width", 0.005f);
        trailRenderer.material = material;
        
        var lr = lineRenderer.GetComponent<LineRenderer>();
        lr.material = material;
        
        photonView = PhotonView.Get(this);
    }

    public void DrawEnter()
    {
        // usingPen = true;
        trailRenderer.gameObject.SetActive(true);
    }

    [PunRPC]
    public void DrawExit()
    {
        // usingPen = false;
        trailRenderer.gameObject.SetActive(false);

        int positionCount = trailRenderer.positionCount;
        Vector3[] list = new Vector3[positionCount];
        trailRenderer.GetPositions(list);
        
        trailRenderer.Clear();
        
        // var line = lineObj.GetComponent<LineRenderer>();
        // line.positionCount = positionCount;
        // line.SetPositions(list);
        // line.gameObject.SetActive(true);

        SendLine(positionCount, list);
    }
    public void SendLine(int positionCount, Vector3[] positions)
    {
        photonView.RPC("getLineDataAndSet", RpcTarget.All, positionCount, positions);
    }

    [PunRPC]
    void getLineDataAndSet(int positionCount, Vector3[] positions)
    {
        var lineObj = PhotonNetwork.Instantiate("InkPrefab", Vector3.zero, Quaternion.identity);
        lineObj.name = $"{photonView.ViewID}{inkPrefix}({inkNo++})";
        
        var line = lineObj.GetComponent<LineRenderer>();
        line.material = material;
        line.positionCount = positionCount;
        line.SetPositions(positions);
        line.gameObject.SetActive(true);
    }

    // public void Update()
    // {
    //     if (usingPen)
    //     {
    //         vectorData.Add(nib.transform.position);
    //     }
    // }
}
