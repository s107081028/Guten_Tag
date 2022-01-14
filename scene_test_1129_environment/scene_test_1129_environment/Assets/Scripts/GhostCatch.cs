using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PhotonTutorial;

public class GhostCatch : MonoBehaviourPun
{
    public PlayerSpawner playerspawner = null;
    bool tagging;
    private bool can_dash;
    float cnt;
    private Animator m_animator;
    public float dash_speed;
    public int cnt_end;

    //private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerspawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>();
       // rb = GetComponent<Rigidbody>();
        m_animator = gameObject.GetComponent<Animator>();
        tagging = false;
        can_dash = true;
        dash_speed = 7.5f;
        cnt = 0;
        cnt_end = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && (!tagging) && gameObject.GetComponent<SkillController>().sprintPower > 10 
            && gameObject.GetComponent<PlayerController>().speedFactor!=0)
        {
            tagging = true;
            can_dash = false;
            gameObject.GetComponent<SkillController>().sprintPower -= 10;
            m_animator.SetBool("dash", true);
            GetComponent<Rigidbody>().AddForce(transform.forward * dash_speed, ForceMode.VelocityChange);
            GetComponent<PlayerController>().speedFactor *= 1.75f;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (tagging)
        {
            if (cnt <= 1) // signal last (and cool down) for frame
            {
                cnt += Time.deltaTime;
                //GetComponent<Rigidbody>().AddForce(transform.forward * dash_speed ,ForceMode.VelocityChange);
                //gameObject.transform.position += transform.forward * dash_speed;
            }
            else
            {
                m_animator.SetBool("dash", false);
                //m_animator.SetBool("dash", false);
                cnt = 0;
                tagging = false;
                can_dash = true;
                GetComponent<PlayerController>().speedFactor /= 1.75f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Debug.Log(other.name);
        if(tagging && other.tag == "Player")
        {
            // end game
            playerspawner.CallGhostWin();
        }
    }
}
