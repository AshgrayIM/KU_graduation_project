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
    public GameObject InkPlane;
    private PhotonView photonView;

    // 실행 취소
    private List<GameObject> undoHistory = new List<GameObject>();
    // 다시 실행 (10개 까지만 저장하는것으로)
    private List<GameObject> redoHistory;
    
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
        photonView.RPC("GetLineDataAndSet", RpcTarget.All, positionCount, positions, photonView.ViewID);
    }

    public void UndoLine()
    {
        // ^1 = history.Count - 1 = index form end expression
        var lastObj = undoHistory[^1];
        undoHistory.RemoveAt(undoHistory.Count - 1);
        
        photonView.RPC("Delete", RpcTarget.All, lastObj.name);
    }

    #region NetWork

    
    [PunRPC]
    void Draw(bool active)
    {
        trailRenderer.gameObject.SetActive(active);
    }

    [PunRPC]
    void Delete(string lineObjName)
    {
        //TODO : 이름 말고 다른 방법 생각해보기
        Destroy(GameObject.Find(lineObjName));
    }

    [PunRPC]
    void GetLineDataAndSet(int positionCount, Vector3[] positions, int drawer)
    {
        // Debug.Log(drawer);
        trailRenderer.Clear();
        
        Vector3 average = Vector3.zero;
        for (int i = 0; i < positionCount; i++){
            average += positions[i];
        }

        var containObj = PhotonNetwork.Instantiate("InkPlane", average / positionCount, Quaternion.identity);
        var lineObj = PhotonNetwork.Instantiate("InkPrefab", Vector3.zero, Quaternion.identity);
        containObj.name = $"{inkPrefix}({photonView.ViewID}" + "/" + $"{inkNo++})";
        lineObj.name = $"{inkPrefix}({photonView.ViewID}" + "/" + $"{inkNo++})";
        lineObj.transform.parent = containObj.transform;

        var line = lineObj.GetComponent<LineRenderer>();
        line.alignment = LineAlignment.TransformZ;
        line.material = material;
        line.positionCount = positionCount;
        line.SetPositions(positions);
        line.gameObject.SetActive(true);
        line.gameObject.tag = "Ink";

        // 내가 그린것만 그림 히스토리에 추가
        // if (drawer == photonView.ViewID)
        // {
        //     Debug.Log(lineObj);
        //     undoHistory.Add(lineObj);
        // }
    }

    #endregion
}
