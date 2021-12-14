using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillController : MonoBehaviourPun
{
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

    private bool sprint = false;
    public float sprintPower = 30f;
    public float sprintMaxPower = 30f;


    // OTHER GAMEOBJECTS
    GameObject prefab;
    GameObject prefab2;
    GameObject prefab3;

    public GameObject bullet;
    public GameObject bullet2;
    public GameObject bullet3;    

    Vector3 prefabPosition;
    //public GameObject GFX;

    // For invisible
    // [SerializeField]
    // public SkinnedMeshRenderer[] renderers;

    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        
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
        skill3Cooldown -= Time.deltaTime;
        skill4Cooldown -= Time.deltaTime;
        skill1Cooldown = Mathf.Clamp(skill1Cooldown, 0f, skill1Speed);
        skill2Cooldown = Mathf.Clamp(skill2Cooldown, 0f, skill2Speed);
        skill3Cooldown = Mathf.Clamp(skill3Cooldown, 0f, skill3Speed);
        skill4Cooldown = Mathf.Clamp(skill4Cooldown, 0f, skill4Speed);

        // SPRINT POWER CALCULATION
        if (sprint) {
            sprintPower -= (6.0f) * Time.deltaTime;      // RUN 5 SECONDS
        }
        else {
            sprintPower += (3.0f) * Time.deltaTime;      // RECOVER 10 SECONDS
        }
        sprintPower = Mathf.Clamp(sprintPower, 0f, sprintMaxPower);
    }

    void FixedUpdate()
    {
        if(photonView.IsMine){
            GameObject playerCamera = GameObject.Find("Main Camera");
            // SKILL1 : SLOW
            if (Input.GetKey(KeyCode.E) && skill1Cooldown <= 0f) {
                if (playerController.aiming)
                {
                    prefab = PhotonNetwork.Instantiate(bullet.name, transform.Find("target").position, Quaternion.identity);
                    prefab.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
                    skill1Cooldown = skill1Speed;
                }
                else
                {
                    prefab = PhotonNetwork.Instantiate(bullet.name, prefabPosition, Quaternion.identity);
                    prefab.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
                    skill1Cooldown = skill1Speed;
                }

                // SKILL ANIMAITON
                // p_animator.SetTrigger("Attack1");
            }

            // SKILL2 : FREEZE
            if (Input.GetKey(KeyCode.R) && skill2Cooldown <= 0f) {
                if (playerController.aiming)
                {
                    prefab2 = PhotonNetwork.Instantiate(bullet2.name, transform.Find("target").position, Quaternion.identity);
                    prefab2.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
                    skill2Cooldown = skill2Speed;
                }
                else
                {
                    prefab2 = PhotonNetwork.Instantiate(bullet2.name, prefabPosition, Quaternion.identity);
                    prefab2.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
                    skill2Cooldown = skill2Speed;
                }
            }

            // SKILL3 : CHAOS
            if (Input.GetKey(KeyCode.T) && skill3Cooldown <= 0f) {
                if (playerController.aiming)
                {
                    prefab3 = PhotonNetwork.Instantiate(bullet3.name, transform.Find("target").position, Quaternion.identity);
                    prefab3.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 800f);
                    skill3Cooldown = skill3Speed;
                } else
                {
                    prefab3 = PhotonNetwork.Instantiate(bullet3.name, prefabPosition, Quaternion.identity);
                    prefab3.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
                    skill3Cooldown = skill3Speed;
                }
            }
        }

        // // SKILL4 : STEALTH
        // if (Input.GetKey(KeyCode.Y) && skill4Cooldown <= 0f) {
        //     SKILL4();
        //     skill4Cooldown = skill4Speed;
        // }

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
        if (col.gameObject.tag == "Skill1") {
            Skill1();
        }

        if (col.gameObject.tag == "Skill2") {
            Skill2();
        }

        if (col.gameObject.tag == "Skill3") {
            Skill3();
        }
    }
    
    // HIT BY SKILL1 : SLOW
    public void Skill1()
    {
        playerController.speedFactor = 0.5f;
        StartCoroutine(DoResetSkill1Factor(skill1Delay));
    }

    IEnumerator DoResetSkill1Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.speedFactor = 1f;
    }

    // HIT BT SKILL2 : FREEZE    
    public void Skill2()
    {
        playerController.speedFactor = 0f;
        StartCoroutine(DoResetSkill2Factor(skill2Delay));
    }

    IEnumerator DoResetSkill2Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.speedFactor = 1f;
    }

    // HIT BY SKILL3 : CHAOS
    public void Skill3()
    {
        playerController.directionFactor = -1;
        StartCoroutine(DoResetSkill3Factor(skill3Delay));
    }

    IEnumerator DoResetSkill3Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.directionFactor = 1;
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
