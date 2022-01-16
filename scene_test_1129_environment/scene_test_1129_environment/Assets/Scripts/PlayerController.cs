using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Cinemachine;
public class PlayerController : MonoBehaviourPun
{
    private Rigidbody rb;
    public float speed;
    public float jumpSpeed = 7f;
    public bool TeleportAvailable;
    //public GameObject canva;

    //public GameObject canva_lose;
    //public PlayerManager playerManager = null;
    [SerializeField] private GameObject playerCamera = null;
    bool can_jump;

    bool end;

    public float speedFactor = 1f;
    public float directionFactor = 1f;

    public bool aiming = false;
    GameObject cine1;
    GameObject cine2;
    public Texture2D cursorTexture;

    private Animator m_animator;
    /*
    void Awake(){
        m_animator = gameObject.GetComponent<Animator>();
        
    }*/
    void Start()
    {
        //Debug.Log("hi");

        playerCamera = GameObject.Find("Main Camera");
        TeleportAvailable = false;
        can_jump = true;
        end = false;



        if (photonView.IsMine) {
            SetCusor();
            //Camera.main.GetComponent<CameraCtrl>().player = this.gameObject;
            //Camera.main.GetComponent<CameraCtrl>().setCameraOffset();
            cine1 = GameObject.Find("CM FreeLook1");
            cine2 = GameObject.Find("CM FreeLook2");
            cine1.GetComponent<CinemachineFreeLook>().Follow = transform;
            cine1.GetComponent<CinemachineFreeLook>().LookAt = transform;
            //cine2.GetComponent<CinemachineFreeLook>().Follow = transform;
            //cine2.GetComponent<CinemachineFreeLook>().LookAt = transform;
            cine2.GetComponent<CinemachineFreeLook>().Follow = transform.Find("target");
            cine2.GetComponent<CinemachineFreeLook>().LookAt = transform.Find("target");
            cine1.SetActive(false);
            cine2.SetActive(false);
           
            cine1.transform.position = transform.position - (transform.forward * cine1.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius) + (cine1.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Height * Vector3.up);
            cine2.transform.position = transform.Find("target").position - (transform.forward * cine2.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius) + (cine2.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Height * Vector3.up);
            cine1.SetActive(true);
            m_animator = gameObject.GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();

            if (PhotonNetwork.LocalPlayer.IsMasterClient) {
                SetMask(10);                // CAN NOT SEE LAYER 10
            }
            else if (!PhotonNetwork.LocalPlayer.IsMasterClient) {
                SetMask(9);                 // CAN NOT SEE LAYER 9
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if (photonView.IsMine)
            {
                TakeInput();
            
            
           }
    }
    void TakeInput()
    {
        float x = Input.GetAxis("Horizontal") * directionFactor;
        float z = Input.GetAxis("Vertical") * directionFactor;

        //  aim input
        if (Input.GetMouseButtonDown(1) && speedFactor!=0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            SetCusor();
            aiming = true;
            cine2.SetActive(true);
            cine1.SetActive(false);
            //cine1.transform.rotation = playerCamera.transform.rotation;
        }
        else if (Input.GetMouseButtonUp(1) || speedFactor==0)
        {
            Cursor.lockState = CursorLockMode.None;

            aiming = false;
            cine1.SetActive(true);
            cine2.SetActive(false);
            //cine2.transform.rotation = playerCamera.transform.rotation;
        }


        if (aiming)
        {


            //transform.forward = playerCamera.transform.forward;

            Vector3 curRotation;// = transform.rotation;
            curRotation = playerCamera.transform.rotation.eulerAngles;
            curRotation.x = 0;

            transform.rotation = Quaternion.Lerp( Quaternion.Euler(curRotation), transform.rotation, 0.1f);// curRotation;
            //transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5f , Vector3.up);

            //cine1.transform.rotation = transform.rotation;
        }
        else
        {
            cine2.transform.position = transform.Find("target").position - (transform.forward * cine2.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius) + (cine2.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Height * Vector3.up);
            //cine2.transform.rotation = transform.rotation;
        }


        // run input
        if (can_jump)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_animator.SetBool("rush", true);
                gameObject.GetComponent<SkillController>().sprint = true;
                speed = 11;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_animator.SetBool("rush", false);
            gameObject.GetComponent<SkillController>().sprint = false;
            speed = 7;
        }

        // move input
        Vector3 rbv = rb.velocity;
        if ((x != 0 || z != 0))//!m_animator.GetBool("dash") && 
        {
            rbv = new Vector3(playerCamera.transform.forward.x*z + playerCamera.transform.right.x*x
            , 0f, playerCamera.transform.forward.z*z+playerCamera.transform.right.z*x).normalized * speed
            + Vector3.up * rbv.y;
            //rb.velocity *= speedFactor;
            rbv = new Vector3(rbv.x * speedFactor, rbv.y, rbv.z * speedFactor);
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, rbv.x, 0.3f),
                                      rbv.y,
                                      Mathf.Lerp(rb.velocity.z, rbv.z, 0.3f));
        }
        if((Mathf.Abs(x) > 0.1 || Mathf.Abs(z) > 0.1) && (!aiming) && speedFactor!=0)
        {
            this.transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.x , rb.velocity.z),0);
        }

        // jump input
        if((speedFactor!=0) && Input.GetKey(KeyCode.Space) && can_jump){
            m_animator.SetBool("jump", true);
            rbv.y = jumpSpeed;
            rb.velocity = rbv;
            can_jump = false;
        }else{
            // if(can_jump){
            //     rbvy = -6;
            // }
            // rbv = rb.velocity;
            // rbv.y = rbvy;
            // rb.velocity = rbv;
        }


        // walk animate
        if (Mathf.Abs(rb.velocity.x)<=1f && Mathf.Abs(rb.velocity.z) <= 1f)
        {           
            m_animator.SetFloat("Speed", 0.0f);
        }
        else
        {            
            m_animator.SetFloat("Speed", 0.5f);
        }
        /*
        Quaternion rt = playerCamera.transform.rotation;
        Vector3 goback = playerCamera.transform.forward*10f;
        goback.y = 0f;
        playerCamera.GetComponent<Transform>().position = new Vector3(transform.position.x, 6.5f, transform.position.z) 
            - goback;
        playerCamera.transform.rotation = rt;*/


        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision CollisionObject){
        if(photonView.IsMine){
            if(CollisionObject.collider.gameObject.tag=="Plane"){
                can_jump = true;
                m_animator.SetBool("jump", false);
            }

            // if(CollisionObject.collider.gameObject.name=="Sphere" && end==false){
            //     canva.SetActive(true);
            //     end = true;
            // }

            // if(CollisionObject.collider.gameObject.name=="Enemy" && end==false){
            //     canva_lose.SetActive(true);
            //     end = true;
            // }
        }

        //print(CollisionObject.collider.gameObject.name);
    }

    void OnCollisionStay(Collision CollisionObject)
    {
        if (photonView.IsMine)
        {
            //if (CollisionObject.collider.gameObject.name == "Plane")
            {
                can_jump = true;
                m_animator.SetBool("jump", false);
            }

            // if(CollisionObject.collider.gameObject.name=="Sphere" && end==false){
            //     canva.SetActive(true);
            //     end = true;
            // }

            // if(CollisionObject.collider.gameObject.name=="Enemy" && end==false){
            //     canva_lose.SetActive(true);
            //     end = true;
            // }
        }

        //print(CollisionObject.collider.gameObject.name);
    }

    void OnCollisionExit(Collision CollisionObject)
    {
        if(photonView.IsMine){
            if(CollisionObject.gameObject.tag == "plane")
            {
                can_jump = false;
                m_animator.SetBool("jump", false);
            }
        }
    }

    public void SetEnd(){
        end = false;
    }
    /*Set cursor*/
    void SetCusor() {
        
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

    }

    void SetMask(int n)
    {
        playerCamera.GetComponent<Camera>().cullingMask = playerCamera.GetComponent<Camera>().cullingMask ^ (1 << n);
    }
}
