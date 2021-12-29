using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroySkill3 : MonoBehaviourPun
{
    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!photonView.IsMine) return;
            PhotonNetwork.Destroy(gameObject);
        }

    }
}