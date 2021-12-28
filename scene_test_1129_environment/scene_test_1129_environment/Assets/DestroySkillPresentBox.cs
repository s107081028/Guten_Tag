using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroySkillPresentBox : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(!photonView.IsMine) return;
        PhotonNetwork.Destroy(gameObject);
    }
}
