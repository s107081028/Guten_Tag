using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skill_Chaos : MonoBehaviourPun
{
    private SkillController skillController;
    public GameObject skill3_present_box_master;
    public GameObject skill3_present_box_client;
    private GameObject prefab3;
    public int maximum_present_box_number = 5;
    public GameObject[] present_box_array;
    public int n;

    void Start()
    {
        skillController = gameObject.GetComponent<SkillController>();
        // skillController = gameObject.transform.parent.gameObject.GetComponent<"SkillController">();      // USING PREFAB ADD UNDER PLAYER, GET ACCESS OF THE PLAYER INFORMATION
    }

    void FixedUpdate()
    {
        if (photonView.IsMine) {
            // SKILL3 : CHAOS
            if (Input.GetKey(KeyCode.E) && skillController.skill1Cooldown <= 0f) {
                present_box_array = GameObject.FindGameObjectsWithTag("Skill3");        // CAN CHANGE TAG
                n = 0;
                foreach (GameObject present_box in present_box_array) {
                    if (present_box.GetComponent<PhotonView>().IsMine) {
                        n++;
                    }
                }
                if (n < maximum_present_box_number) {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient) {
                        prefab3 = PhotonNetwork.Instantiate(skill3_present_box_master.name, new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * -5f, Quaternion.identity);
                    }
                    else {
                        prefab3 = PhotonNetwork.Instantiate(skill3_present_box_client.name, new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * -5f, Quaternion.identity);
                    }
                    skillController.skill1Cooldown = skillController.skill1Speed;
                }
                //prefab3 = PhotonNetwork.Instantiate(skill3_present_box.name, new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * -5f, Quaternion.identity);
                // skillController.skill1Cooldown = skillController.skill1Speed;
            }
        }
    }

}