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
        if (col.gameObject.tag == "Player") {
            if (photonView.IsMine == col.gameObject.GetComponent<PhotonView>().IsMine) {
                PhotonNetwork.Destroy(photonView.gameObject);
            }
            else {
                photonView.RPC(nameof(DestroyPresentBox), RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    public void DestroyPresentBox()
    {
        PhotonNetwork.Destroy(gameObject);         
    }
}
