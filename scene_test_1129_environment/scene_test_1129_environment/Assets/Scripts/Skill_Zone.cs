using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skill_Zone : MonoBehaviourPun
{
    private SkillController skillController;
    public GameObject skill4_zone;
    private GameObject prefab5;
    void Start()
    {
        skillController = gameObject.GetComponent<SkillController>();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine) {
            // SKILL4 : ZONE
            if (Input.GetKey(KeyCode.E) && skillController.skill1Cooldown <= 0f) {
                prefab5 = PhotonNetwork.Instantiate(skill4_zone.name, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);
                skillController.skill1Cooldown = skillController.skill1Speed;
            }
        }
    }
}