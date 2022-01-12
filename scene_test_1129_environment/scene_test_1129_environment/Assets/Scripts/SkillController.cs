using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillController : MonoBehaviourPun
{
    GameObject playerCamera;
    // FACTOR
    public float speedFactor = 1f;
    public int directionFactor = 1;
    public float skill1Cooldown = 0f;
    public float skill2Cooldown = 0f;
    public float skill3Cooldown = 0f; 
    public float skill4Cooldown = 0f; 

    public float skill1Speed = 1f;
    public float skill2Speed = 3f;
    public float skill3Speed = 10f;
    public float skill4Speed = 10f;

    public float skill1Delay = 5f;
    public float skill2Delay = 5f;
    public float skill3Delay = 5f;
    public float skill4Delay = 5f;

    public bool sprint = false;
    public float sprintPower = 30f;
    public float sprintMaxPower = 30f;

    public GameObject dizzyeffect;

    // OTHER GAMEOBJECTS
    GameObject prefab;
    GameObject prefab2;
    GameObject prefab3;
    GameObject prefab4;
    GameObject dizzyprefab;

    private bool debuff;
    public GameObject bullet;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject chaoseffect;
    private GameObject chaosprefab;


    public GameObject Eggeffect;
    public GameObject Egg3D;
    public GameObject Snoweffect;
    public GameObject EggCrackeffect;
    GameObject Snoweffectprefab;
    GameObject Eggprefab;
    GameObject EggprefabONFace;
    GameObject EggCrackprefab;

    Vector3 prefabPosition;
    //public GameObject GFX;

    // For invisible
    // [SerializeField]
    // public SkinnedMeshRenderer[] renderers;

    public PlayerController playerController;
    private Animator m_animator;

    private int item;
    private int bulletNum;
    public GameObject[] ItemPrefab;
    public bool punishing;
    public float punishing_end;

    private FillAmountController fillAmountController;
    public bool IsOnZone;
    public GameObject PresentBox_ParticleEffect;
    private GameObject PresentBox_ParticleEffect_prefab;
    public float fallSpeed;
    private float preFallSpeed;

    void Start()
    {
        playerCamera = Camera.main.gameObject; //GameObject.Find("Main Camera");
        playerController = GetComponent<PlayerController>();
        m_animator = gameObject.GetComponent<Animator>();
        debuff = false;
        item = -1;
        bulletNum = 0;
        punishing = false;
        punishing_end = 15;
        IsOnZone = false;

        fillAmountController = gameObject.GetComponent<FillAmountController>();
        preFallSpeed = 0;
        fallSpeed = -10;
    }

    void Update()
    {

        
        // TRASH

        // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // if (isGrounded && velocity.y < 0) {
        //     velocity.y = -2f;
        //     JumpAnimationEnd();
        // }

        // // MOVE
        // float x = Input.GetAxis("Horizontal") * directionFactor;
        // float z = Input.GetAxis("Vertical") * directionFactor;
        // Vector3 move = transform.right * x + transform.forward * z;
        // controller.Move(move * speed * speedFactor * Time.deltaTime);

        // // ANIMATOR
        // v = move.magnitude;
        // p_animator.SetFloat("Speed", v);

        
        // // JUMP
        // if (Input.GetButtonDown("Jump") && isGrounded) {
        //     velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //     JumpAnimationStart();
        // }
        // velocity.y += gravity * Time.deltaTime;
        // controller.Move(velocity * Time.deltaTime);

        // if (!isGrounded) {
        //     JumpAnimationEnd();
        // }


        if(!photonView.IsMine){
            return;
        }



        // CALCULATE SKILL COOL DOWN
        //prefabPosition = transform.Find("target").position;
        prefabPosition = transform.position + transform.up * 0.5f + transform.forward * 1.0f;
        // prefabPosition = transform.position + controller.center + transform.forward * 1f;

         skill1Cooldown -= Time.deltaTime;
         skill2Cooldown -= Time.deltaTime;
        //skill3Cooldown -= Time.deltaTime;
        //skill4Cooldown -= Time.deltaTime;
         skill1Cooldown = Mathf.Clamp(skill1Cooldown, 0f, skill1Speed);
         skill2Cooldown = Mathf.Clamp(skill2Cooldown, 0f, skill2Speed);
        //skill3Cooldown = Mathf.Clamp(skill3Cooldown, 0f, skill3Speed);
        //skill4Cooldown = Mathf.Clamp(skill4Cooldown, 0f, skill4Speed);

        // SPRINT POWER CALCULATION
        if (sprint) {
            sprintPower -= (5.0f) * Time.deltaTime;      // RUN 5 SECONDS      //buffed by shaoting
        }
        else {
            sprintPower += (5.0f) * Time.deltaTime;      // RECOVER ? SECONDS  //buffed by shaoting
        }
        sprintPower = Mathf.Clamp(sprintPower, 0f, sprintMaxPower);

        // CW
        // SKILL3 : CHAOS EFFECT TRACE PLAYER
        if (chaosprefab != null) {
            chaosprefab.transform.position = transform.position + transform.up;
       
        }

        if (dizzyprefab != null)
        {
            dizzyprefab.transform.position = transform.position + transform.up * 1.5f;
        }

        if (Eggprefab != null)
        {
            Eggprefab.transform.position = transform.position + transform.up * 1.5f;
        }

        if (EggprefabONFace != null)
        {
            Vector3 temp = playerCamera.transform.position + playerCamera.transform.forward * 1.7f;
            temp.x = Mathf.Lerp(EggprefabONFace.transform.position.x, temp.x, 3f);
            temp.y = Mathf.Lerp(EggprefabONFace.transform.position.y, temp.y, 3f);
            temp.z = Mathf.Lerp(EggprefabONFace.transform.position.z, temp.z, 3f);
            EggprefabONFace.transform.position = temp;
        }

        if (Snoweffectprefab != null)
        {
            Snoweffectprefab.transform.position = transform.position + transform.up * 2.3f;
        }
        
        // SKILL4 : ZONE DETERMINE RECOVER TIMING
        if (IsOnZone && GameObject.FindGameObjectWithTag("Skill4") == null) {
            playerController.speedFactor = 1f;
            // Debug.Log("cw Skill4 : recover");
            IsOnZone = false;
        }
        preFallSpeed = gameObject.GetComponent<Rigidbody>().velocity.y;
    }

    void FixedUpdate()
    {
        if(photonView.IsMine){
            GameObject playerCamera = GameObject.Find("Main Camera");
            // SKILL1 : SLOW
            // if (Input.GetKey(KeyCode.E) && skill1Cooldown <= 0f) {
            //     if (playerController.aiming)
            //     {
            //         //prefab = PhotonNetwork.Instantiate(bullet.name, transform.Find("target").position, Quaternion.identity);
            //         //prefab.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
            //         skill1Cooldown = skill1Speed;
            //     }
            //     else
            //     {
            //         //prefab = PhotonNetwork.Instantiate(bullet.name, prefabPosition, Quaternion.identity);
            //         //prefab.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
            //         skill1Cooldown = skill1Speed;
            //     }

            // //     // SKILL ANIMAITON
            // //     // p_animator.SetTrigger("Attack1");
            //  }

            // // SKILL2 : FREEZE
            // if (Input.GetKey(KeyCode.R) && skill2Cooldown <= 0f) {
            //     if (playerController.aiming)
            //     {
            //         prefab2 = PhotonNetwork.Instantiate(bullet2.name, transform.Find("target").position, Quaternion.identity);
            //         prefab2.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
            //         skill2Cooldown = skill2Speed;
            //     }
            //     else
            //     {
            //         prefab2 = PhotonNetwork.Instantiate(bullet2.name, prefabPosition, Quaternion.identity);
            //         prefab2.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
            //         skill2Cooldown = skill2Speed;
            //     }
            // }

            // // SKILL3 : CHAOS
            // if (Input.GetKey(KeyCode.T) && skill3Cooldown <= 0f) {
            //     if (playerController.aiming)
            //     {
            //         prefab3 = PhotonNetwork.Instantiate(bullet3.name, transform.Find("target").position, Quaternion.identity);
            //         prefab3.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
            //         skill3Cooldown = skill3Speed;
            //     } else
            //     {
            //         prefab3 = PhotonNetwork.Instantiate(bullet3.name, prefabPosition, Quaternion.identity);
            //         prefab3.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
            //         skill3Cooldown = skill3Speed;
            //     }
            // }

            m_animator.SetBool("Attack", false);
            if (Input.GetKeyDown(KeyCode.R) && playerController.speedFactor!=0) {
                print(item);
                if(item != -1 && bulletNum > 0){
                    if (playerController.aiming)
                    {
                        m_animator.SetBool("Attack", true);
                        prefab4 = PhotonNetwork.Instantiate(ItemPrefab[item].name, transform.Find("target").position + playerCamera.transform.forward * 6.0f, Quaternion.identity);
                        prefab4.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 1500f);
                        // skill3Cooldown = skill3Speed;
                    } else
                    {
                        m_animator.SetBool("Attack", true);                        
                        prefab4 = PhotonNetwork.Instantiate(ItemPrefab[item].name, prefabPosition + transform.forward * 6.0f, Quaternion.identity);
                        prefab4.GetComponent<Rigidbody>().AddForce(transform.forward * 1500);
                        // skill3Cooldown = skill3Speed;
                    }
                    bulletNum -= 1;
                    gameObject.GetComponent<FillAmountController>().RefreshBulletNum(bulletNum);
                }
            }
            // Too Exhausted 
            m_animator.SetBool("exhausted", false);
            if(sprintPower >= 15.0f){
                if(punishing ==true){
                    playerController.speedFactor = 1.0f;
                }
                punishing = false;
            } else if( sprintPower == 0f){
                punishing = true;
                m_animator.SetBool("exhausted", true);
            }

            if(punishing){
                playerController.speedFactor = 0f;  
            } //else if(!m_animator.GetBool("dizzy")) playerController.speedFactor = 1.0f;

            if(bulletNum == 0) item = -1;
        }

         // SKILL4 : STEALTH
         //if (Input.GetKey(KeyCode.Y) && skill4Cooldown <= 0f) {
         //    SKILL4();
         //    skill4Cooldown = skill4Speed;
         //}

        // SPRINT
        /*
        if (Input.GetKey(KeyCode.LeftShift) && (sprintPower > 0f)) {
            sprint = true;
            playerController.speedFactor = 1.5f;
            // Debug.Log("sprint");
        }
        else {
            sprint = false;
            playerController.speedFactor = 1f;
            // Debug.Log("not sprint");
        }*/
    }


///////////////////////////////////////////////////////////////////////////////////////////////////
// SKILL BEGIN
///////////////////////////////////////////////////////////////////////////////////////////////////

    void OnCollisionEnter(Collision col)
    {
        if (!photonView.IsMine) return;
        if (col.gameObject.tag == "Skill1") {
            if(!debuff) Skill1();
        }

        if (col.gameObject.tag == "Skill2") {
            if(!debuff){
                Skill2();
                print("Skill2");
            }
        }

        if (col.gameObject.tag == "Skill3") {
            // ONLY ENEMY'S SKILL IS TRIGGER
            if (!col.gameObject.GetComponent<PhotonView>().IsMine) {
                if(!debuff) Skill3();
                if(photonView.IsMine) {
                    PresentBox_ParticleEffect_prefab = PhotonNetwork.Instantiate(PresentBox_ParticleEffect.name, col.gameObject.transform.position + transform.up * 0.6f, Quaternion.identity);
                    PresentBox_ParticleEffect_prefab.GetComponent<ParticleSystem>().Play();
                }
            }
        }

        if (col.gameObject.tag == "Skill4")
        {
            if (!debuff) Skill4();
        }
        if(preFallSpeed <= fallSpeed) {
            print("fall");
            if(!debuff) Skill2();
        }
    }

    // HIT BY SKILL1 : SLOW
    public void Skill1()
    {
        playerController.speedFactor = 0.5f;
        debuff = true;
        Snoweffectprefab = PhotonNetwork.Instantiate(Snoweffect.name, transform.position + transform.up * 2.3f, Quaternion.identity);
        StartCoroutine(DoResetSkill1Factor(skill1Delay));
    }

    IEnumerator DoResetSkill1Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.speedFactor = 1f;
        debuff = false;
        PhotonNetwork.Destroy(Snoweffectprefab);
    }

    // HIT BT SKILL2 : FREEZE    
    public void Skill2()
    {
        playerController.speedFactor = 0f;
        debuff = true;
        if(photonView.IsMine) dizzyprefab = PhotonNetwork.Instantiate(dizzyeffect.name, transform.position + transform.up * 1.5f, new Quaternion(0, 90, 90, 0));
        m_animator.SetBool("dizzy", true);
        StartCoroutine(DoResetSkill2Factor(skill2Delay));
    }

    IEnumerator DoResetSkill2Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.speedFactor = 1f;
        debuff = false;
        m_animator.SetBool("dizzy", false);
        if(photonView.IsMine) PhotonNetwork.Destroy(dizzyprefab);
    }

    // HIT BY SKILL3 : CHAOS
    public void Skill3()
    {
        playerController.directionFactor = -1f;
        debuff = true;
        if(photonView.IsMine) {
            chaosprefab = PhotonNetwork.Instantiate(chaoseffect.name, transform.position + transform.up, Quaternion.identity);
            chaosprefab.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(DoResetSkill3Factor(skill3Delay));
    }

    IEnumerator DoResetSkill3Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.directionFactor = 1f;
        debuff = false;
    }


    // Hit By Egg
    public void Skill4()
    {
        debuff = true;
        if (photonView.IsMine)
        {
            EggCrackprefab = PhotonNetwork.Instantiate(EggCrackeffect.name, transform.position + transform.up, Quaternion.identity);
            Eggprefab = PhotonNetwork.Instantiate(Eggeffect.name, transform.position + transform.up *1.5f, Quaternion.identity);
            EggprefabONFace = Instantiate(Eggeffect, playerCamera.transform.position + playerCamera.transform.forward*2f, Quaternion.identity);
        }
        StartCoroutine(DoResetSkill4Factor(skill4Delay));
    }

    IEnumerator DoResetSkill4Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (photonView.IsMine)
        {
            Destroy(EggprefabONFace);
            PhotonNetwork.Destroy(Eggprefab);
            //PhotonNetwork.Destroy(EggCrackprefab);
        }
        debuff = false;
    }

    // CW
    // TRIGGER SKILL4 : ZONE
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Skill4") {
            if (photonView.IsMine) {
                IsOnZone = true;
                if (col.gameObject.GetComponent<PhotonView>().IsMine) {
                    playerController.speedFactor = 1.5f;
                }
                else {
                    playerController.speedFactor = 0.7f;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Skill4") {
            if (photonView.IsMine) {
                IsOnZone = false;
                playerController.speedFactor = 1f;
            }
        }
    }

    public void PickUpCoin(int i)
    {
        item = i;
        bulletNum = 5;
        fillAmountController.PickUpSkill(item);
        fillAmountController.RefreshBulletNum(bulletNum);
    }

    // SKILL4 : STEALTH
    // public void SKILL4()
    // {
    //     //GFX.SetActive(false);

    //     foreach (SkinnedMeshRenderer renderer in renderers) {
    //         renderer.enabled = false;
    //     }
        
    //     StartCoroutine(DoSetActive(skill4Delay));
    // }

    // IEnumerator DoSetActive(float delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     foreach (SkinnedMeshRenderer renderer in renderers) {
    //         renderer.enabled = true;
    //     }
        
    //     GFX.SetActive(true);
    // }


///////////////////////////////////////////////////////////////////////////////////////////////////
// SKILL END
///////////////////////////////////////////////////////////////////////////////////////////////////
}