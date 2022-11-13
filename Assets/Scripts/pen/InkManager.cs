using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private PhotonView photonView;

    public void Awake()
    {    
        photonView = PhotonView.Get(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            photonView.RPC("DeleteAll", RpcTarget.All, "Deleted All");
        }
    }

    [PunRPC]
    void DeleteAll(string log)
    {
        Debug.LogError(log);
        var inkList = GameObject.FindGameObjectsWithTag("Ink");
        for (int i = 0; i < inkList.Length; i++)
        {
            Destroy(inkList[i]);
        }
    }
}
