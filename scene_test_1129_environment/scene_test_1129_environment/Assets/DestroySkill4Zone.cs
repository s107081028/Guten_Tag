using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroySkill4Zone : MonoBehaviourPun
{
    void Start()
    {        
        gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 10);     // SKILL4 ZONE DELAY
    }

    void Update()
    {
        
    }
}
