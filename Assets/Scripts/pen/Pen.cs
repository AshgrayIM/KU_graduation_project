using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pen : MonoBehaviour
{
    public Material material;
    public TrailRenderer trailRenderer;
    public GameObject lineRenderer;
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
        photonView.RPC("Draw", RpcTarget.All, true);
    }

    [PunRPC]
    public void DrawExit()
    {
        photonView.RPC("Draw", RpcTarget.All, false);

        int positionCount = trailRenderer.positionCount;
        Vector3[] list = new Vector3[positionCount];
        trailRenderer.GetPositions(list);

        SendLine(positionCount, list);
    }
    public void SendLine(int positionCount, Vector3[] positions)
    {
        photonView.RPC("getLineDataAndSet", RpcTarget.All, positionCount, positions);
    }

    [PunRPC]
    void Draw(bool active)
    {
        trailRenderer.gameObject.SetActive(active);
    }

    [PunRPC]
    void getLineDataAndSet(int positionCount, Vector3[] positions)
    {
        trailRenderer.Clear();
        var lineObj = PhotonNetwork.Instantiate("InkPrefab", Vector3.zero, Quaternion.identity);
        lineObj.name = $"{photonView.ViewID}{inkPrefix}({inkNo++})";
        
        var line = lineObj.GetComponent<LineRenderer>();
        line.material = material;
        line.positionCount = positionCount;
        line.SetPositions(positions);
        line.gameObject.SetActive(true);
    }
}
