using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DestroyBullet : MonoBehaviourPun
{
    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnCollisionEnter(Collision col)
    {
        if(!photonView.IsMine) return;
        print("bullethit" + col.gameObject.name);
        PhotonNetwork.Destroy(gameObject);
    }
}
