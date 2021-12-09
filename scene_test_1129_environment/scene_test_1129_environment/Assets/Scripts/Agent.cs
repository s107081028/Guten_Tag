using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class Agent : MonoBehaviour
{
     private NavMeshAgent m_agent;
    private Animator m_animator;

    public GameObject player;

    public GameObject canva;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   

	// Use this for initialization
    void Awake()
    {
        m_agent = gameObject.GetComponent<NavMeshAgent>();
        m_animator = gameObject.GetComponent<Animator>(); 
    }	
	
	// Update is called once per frame
	void Update () {
        // Change "Speed" of Animator with velocity of agent
        if (m_agent.velocity.magnitude < 0.4f && m_animator != null)
        {           
            m_animator.SetFloat("Speed", 0.0f);
        }
        else
        {            
            m_animator.SetFloat("Speed", 0.5f);
        }
        m_agent.SetDestination(player.transform.position);
	}
    
    void OnCollisionEnter(Collision CollisionObject){

        //print(CollisionObject.collider.gameObject.name);
    }
}
