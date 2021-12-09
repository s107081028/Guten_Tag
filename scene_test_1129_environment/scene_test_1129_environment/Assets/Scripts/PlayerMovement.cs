// CHEN WEI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // FACTOR
    public float speed = 10f;
    public float gravity = -29.4f;
    public float jumpHeight = 1.5f;
    public float groundDistance = 0.1f;
    bool isGrounded;
    private float v;
    public float skill1Cooldown = 0f;
    public float skillSpeed = 1f;

    public float speedFactor = 1f;

    public float skill1Delay = 5f;


    // OTHER GAMEOBJECTS
    public Transform groundCheck;
    public CharacterController controller;
    public LayerMask groundMask;
    Vector3 velocity;
    private Animator p_animator;
    GameObject prefab;
    public GameObject bullet;
    Vector3 prefabPosition;


    void Start()
    {
        p_animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            JumpAnimationEnd();              ////
        }

        // MOVE
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * speedFactor * Time.deltaTime);

        // ANIMATOR
        v = move.magnitude;
        p_animator.SetFloat("Speed", v);

        
        // JUMP
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            JumpAnimationStart();
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (!isGrounded) {
            JumpAnimationEnd();
        }

        // CALCULATE SKILL COOL DOWN
        prefabPosition = transform.position + controller.center + transform.forward * 1f;
        skill1Cooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // SKILL1
        if (Input.GetKey(KeyCode.E) && skill1Cooldown <= 0f) {
            prefab = Instantiate(bullet, prefabPosition, Quaternion.identity);
            prefab.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);
            skill1Cooldown = skillSpeed;

            // SKILL ANIMAITON
            p_animator.SetTrigger("Attack1");
        }
    }


    // JUMP ANIMATION
    public void JumpAnimationStart()
    {
        p_animator.SetBool("IsJump", true);
    }

    public void JumpAnimationEnd()
    {
        p_animator.SetBool("IsJump", false);
    }


    // SKILL1 : SLOW
    public void Skill1()
    {
        speedFactor = 0.5f;
        StartCoroutine(DoResetFactor(skill1Delay));
    }

    IEnumerator DoResetFactor(float delay)
    {
        yield return new WaitForSeconds(delay);
        speedFactor = 1f;
    }

    // HIT BY SKILL1
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Skill1") {
            Skill1();
        }
    }
}
