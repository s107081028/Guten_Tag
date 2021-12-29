using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroySkill4 : MonoBehaviourPun
{
    public float skill4time = 10f;

    void Start()
    {
        StartCoroutine(DoDestroySkill4(skill4time));
    }

    IEnumerator DoDestroySkill4(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);

    }

    // void OnTriggerEnter(Collider col) {
    //     if (col.gameObject.tag == "Player") {
    //         if (!photonView.IsMine) {
    //             Skill4_Debuff();
    //         }
    //         else {
    //             Skill4_Buff();
    //         }
    //     }
    // }
    
    // void OnTriggerExit(Collider col) {
    //     if (col.gameObject.tag == "Player") {
    //         if (!photonView.IsMine) {
    //             Skill4_Debuff_Recover();
    //         }
    //         else {
    //             Skill4_Buff_Recover();
    //         }
    //     }
    // }


}
