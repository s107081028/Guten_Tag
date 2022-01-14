using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroySkillPresentBox : MonoBehaviourPun
{
    // public GameObject PresentBox_ParticleEffect;
    // private GameObject PresentBox_ParticleEffect_prefab;
//public AudioClip PresentBox_Hit;
    
    void Start()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player") {
            
            PhotonView colPhotonView = col.gameObject.GetComponent<PhotonView>();
            if (colPhotonView == null) return;
            if (photonView.IsMine == colPhotonView.IsMine) {
                // NOTHING HAPPENS
            }
            else {
                /*if(GetComponent<AudioSource>()) 
                    GetComponent<AudioSource>().PlayOneShot(PresentBox_Hit);*/

                // if(photonView.IsMine) {
                //     PresentBox_ParticleEffect_prefab = PhotonNetwork.Instantiate(PresentBox_ParticleEffect.name, transform.position + transform.up * 0.6f, Quaternion.identity);
                //     PresentBox_ParticleEffect_prefab.GetComponent<ParticleSystem>().Play();
                // }
                // StartCoroutine(DoDestroyPresentBox(1.5f));

                //photonView.RPC(nameof(DestroyPresentBox), RpcTarget.Others);
                if (!photonView.IsMine)
                {
                    photonView.RPC(nameof(DestroyPresentBox), RpcTarget.Others);
                }
            }
        }
    }

    [PunRPC]
    public void DestroyPresentBox()
    {
        PhotonNetwork.Destroy(gameObject);         
    }

    // IEnumerator DoDestroyPresentBox(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     photonView.RPC(nameof(DestroyPresentBox), RpcTarget.Others);
    // }

}