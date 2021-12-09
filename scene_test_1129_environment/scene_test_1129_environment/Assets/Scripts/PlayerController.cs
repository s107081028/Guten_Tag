using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody rb;
    public float speed;
    public bool TeleportAvailable;
    //public GameObject canva;

    //public GameObject canva_lose;
    //public PlayerManager playerManager = null;
    [SerializeField] private GameObject playerCamera = null;
    bool can_jump;

    bool end;

    private Animator m_animator;

    void Awake(){
        m_animator = gameObject.GetComponent<Animator>();
        
    }
    void Start()
    {
        //Debug.Log("hi");
        rb = GetComponent<Rigidbody>();
        m_animator = gameObject.GetComponent<Animator>(); 
        playerCamera = GameObject.Find("Main Camera");
        TeleportAvailable = false;
        can_jump = true;
        end = false;

        //if (photonView.IsMine) {
        //    Camera.main.GetComponent<CameraCtrl>().player = this.gameObject;
        //    Camera.main.GetComponent<CameraCtrl>().setCameraOffset();
        //}
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            rb.velocity = new Vector3(playerCamera.transform.forward.x*z+playerCamera.transform.right.x*x
            , 0f, playerCamera.transform.forward.z*z+playerCamera.transform.right.z*x).normalized * speed 
            + Vector3.up * rb.velocity.y;
        }
        if(Mathf.Abs(x) > 0.1 || Mathf.Abs(z) > 0.1)
        {
            this.transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.x , rb.velocity.z),0);
        }

        Vector3 rbv = rb.velocity;
        if(Input.GetKey(KeyCode.Space) && can_jump){
            rbv.y = 5f;
            rb.velocity = rbv;
            can_jump = false;
            //m_animator.SetBool("jump", true);
        }else{
            // if(can_jump){
            //     rbvy = -6;
            // }
            // rbv = rb.velocity;
            // rbv.y = rbvy;
            // rb.velocity = rbv;
        }

        if (Mathf.Abs(rb.velocity.x)<=0.1 && Mathf.Abs(rb.velocity.z) <= 0.1f)
        {           
            m_animator.SetFloat("Speed", 0.0f);
        }
        else
        {            
            m_animator.SetFloat("Speed", 0.5f);
        }

        Quaternion rt = playerCamera.transform.rotation;
        Vector3 goback = playerCamera.transform.forward*10f;
        goback.y = 0f;
        playerCamera.GetComponent<Transform>().position = new Vector3(transform.position.x, 6.5f, transform.position.z) 
            - goback;
        playerCamera.transform.rotation = rt;


        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision CollisionObject){
        if(photonView.IsMine){
            if(CollisionObject.collider.gameObject.tag=="Plane"){
                can_jump = true;
                //m_animator.SetBool("jump", false);
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
            if (CollisionObject.collider.gameObject.name == "Plane")
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

    public void SetEnd(){
        end = false;
    }
}
