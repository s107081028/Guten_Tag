using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
public class Skill : MonoBehaviour {


    float cooldownTime;

    public virtual void triggerSkill() {

    }
}*/
public class Clone : MonoBehaviourPun
{
    [Header("Skill Base")]
    public float skillCooldownTime = 10f;
    public KeyCode triggerKey = KeyCode.C;
    private bool canUseSkill = true;
    private SkillController skillController;
    [Header("Clone")]
    public GameObject soundPrefab;
    public GameObject triggerVFX;
    public GameObject cloneCharacrer;
    public float cloneLivingTime = 5f;
    GameObject spawnedCloneCharacrer;
    


    string[] aniamtorVarNames = { "jump", "rush", "dizzy", "dash", "exhausted" };
    // Start is called before the first frame update
    void Start()
    {

        skillController = GetComponent<SkillController>();
        
        // skin
        cloneCharacrer.GetComponent<CharacterSkinChanger>().changeSkinOfThis = true;
        cloneCharacrer.GetComponent<CharacterSkinChanger>().partNames = gameObject.GetComponent<CharacterSkinChanger>().partNames;
        cloneCharacrer.GetComponent<CharacterSkinChanger>().changeSkinList = gameObject.GetComponent<CharacterSkinChanger>().changeSkinList;
        cloneCharacrer.GetComponent<CharacterSkinChanger>().enabled = true;




 
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(triggerKey) && canUseSkill) {
            triggerSkill();
            skillController.skill1Cooldown = skillController.skill1Speed;
        }
        


        if (spawnedCloneCharacrer) {
            SyncMovement();
            SyncRotation();
            SyncAnimator();
            SyncSprintBar();
            
        }
            
                
    }

    void triggerSkill() {

        CreateClone();
        canUseSkill = false;
        Invoke(nameof(resetSkillCanUse), skillCooldownTime);
    }

    void resetSkillCanUse(){
        canUseSkill = true;

    }

    void CreateClone() {



        photonView.RPC(nameof(InstantiatePartical), RpcTarget.All, transform.position);


        spawnedCloneCharacrer = PhotonNetwork.Instantiate("Skill_Clone_Character", transform.position, Quaternion.identity);
        // Ignore collide
        Physics.IgnoreCollision(spawnedCloneCharacrer.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());


        // Destroy cloned object later
        Invoke(nameof(DestroyClone),cloneLivingTime);
        
    }


    void SyncMovement() {
        Vector3 camera_forword = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Vector3 playerVelocity = new Vector3( GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
        Vector3 cloneVelocity = playerVelocity + 2f * (Vector3.Project(playerVelocity, camera_forword) - playerVelocity);
        if (GetComponent<Rigidbody>().velocity.y > 0.1f)
        {
            cloneVelocity.y = GetComponent<Rigidbody>().velocity.y;
        }
        else
        {
            cloneVelocity.y = spawnedCloneCharacrer.GetComponent<Rigidbody>().velocity.y;
        }

        spawnedCloneCharacrer.GetComponent<Rigidbody>().velocity = cloneVelocity;
        

    }

    void SyncRotation() {
        Vector3 playerVelocity = spawnedCloneCharacrer.GetComponent<Rigidbody>().velocity;
        spawnedCloneCharacrer.transform.forward = new Vector3(playerVelocity.x, 0, playerVelocity.z);

    }

    void SyncAnimator() {
        spawnedCloneCharacrer.GetComponent<Animator>().SetFloat("Speed", GetComponent<Animator>().GetFloat("Speed"));

        

        foreach (string animatorVarName in aniamtorVarNames) {
            
            spawnedCloneCharacrer.GetComponent<Animator>().SetBool(animatorVarName, GetComponent<Animator>().GetBool(animatorVarName));
        }

            
    }

    void SyncSprintBar() {
        spawnedCloneCharacrer.GetComponent<SprintBar>().currentSprintValue = GetComponent<SprintBar>().currentSprintValue;
    }

    void DestroyClone() {

        ShowDestroyVFX();
        DestroyClonedCharacter();
        //
//        Invoke(nameof(DestroyClonedCharacter), 0.5f);

    }

    void ShowDestroyVFX() {
        photonView.RPC(nameof(InstantiatePartical), RpcTarget.All, spawnedCloneCharacrer.transform.position); // show vfx again
    }

    void DestroyClonedCharacter()
    {
        PhotonNetwork.Destroy(spawnedCloneCharacrer);

    }

    [PunRPC]
    void InstantiatePartical(Vector3 pos) {

        Instantiate(triggerVFX, pos + new Vector3(0,1f,0), Quaternion.identity);
        Instantiate(soundPrefab, pos + new Vector3(0,1f,0), Quaternion.identity);
    }
}
