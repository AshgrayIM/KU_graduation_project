using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerHeadMaterialControlFromMic : MonoBehaviour
{
    public GameObject Speaker;
    MeshRenderer meshRenderer;
    Material[] materials;
    AudioSource audioSource;
    


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
        audioSource = Speaker.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.isPlaying){
            materials[0].color = Color.green;
        }
        else{
            materials[0].color = Color.gray;
        }
    }
}
