using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*

public class SkillBase : MonoBehaviourPun {
    KeyCode key;
    Image skillImg;
    float cooldown;
    string skillDescription;

    public virtual void triggerSkill() { 
    }

}*/


public class Invisibility : MonoBehaviourPun
{



    /*All skill will need*/

    KeyCode key;
    Image skillImg;
    float cooldown;
    public GameObject smokeParticle;
    private GameObject spawnPartical;
    public GameObject nameTag;

    public List<GameObject> inviisibleTargets;
    bool isInvisible;
    private SkillController skillController;
    // Start is called before the first frame update
    void Start()
    {
        isInvisible = false;
        skillController = gameObject.GetComponent<SkillController>();
    }



    [PunRPC]
    public void startInvisible(float duration) {

        if (photonView.IsMine)
        {
            spawnPartical = PhotonNetwork.Instantiate(smokeParticle.name, transform.position, Quaternion.identity);
        }
        else {
            nameTag.SetActive(false);
        }


        foreach (GameObject target in inviisibleTargets) {
            SkinnedMeshRenderer renderer = target.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null) continue;
            renderer.enabled = false;
        }
        isInvisible = true;

        Invoke(nameof(restoreVisible), duration);
        Invoke(nameof(destoryMyBiuBiu), 2);
    }

    [PunRPC]
    private void destoryMyBiuBiu() {
        if (spawnPartical == null) return;
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(spawnPartical);
        }

    }



    [PunRPC]
    private void restoreVisible() {
        isInvisible = false;
        foreach (GameObject target in inviisibleTargets)
        {
            SkinnedMeshRenderer renderer = target.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null) continue;
            renderer.enabled = true;
        }
        if (!photonView.IsMine) {
            nameTag.SetActive(true);

        }

    }

    [PunRPC]
    public void hideNameTag(float duration)
    {
        nameTag.SetActive(false);


        Invoke(nameof(showNameTag), duration);
    }
    [PunRPC]
    public void showNameTag()
    {
        nameTag.SetActive(true);

    }


    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.E) && (!isInvisible) && (skillController.skill1Cooldown<=0f))
        {
            skillController.skill1Cooldown = skillController.skill1Speed;
            photonView.RPC(nameof(startInvisible), RpcTarget.All, 3f);
            //photonView.RPC(nameof(hideNameTag), RpcTarget.Others, 10f);
            //startInvisible(10);
        }
        //if(isInvisible )
    }
}
