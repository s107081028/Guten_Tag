using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skill_Chaos : MonoBehaviourPun
{
    private SkillController skillController;
    public GameObject skill3_present_box;
    private GameObject prefab3;


    void Start()
    {
        skillController = gameObject.GetComponent<SkillController>();
        // skillController = gameObject.transform.parent.gameObject.GetComponent<"SkillController">();      // USING PREFAB ADD UNDER PLAYER, GET ACCESS OF THE PLAYER INFORMATION
    }

    void FixedUpdate()
    {
        if (photonView.IsMine) {
            // SKILL3 : CHAOS
            if (Input.GetKey(KeyCode.E) && skillController.skill3Cooldown <= 0f) {
                prefab3 = PhotonNetwork.Instantiate(skill3_present_box.name, new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * -5f, Quaternion.identity);
                skillController.skill3Cooldown = skillController.skill3Speed;
            }
        }
    }

}
