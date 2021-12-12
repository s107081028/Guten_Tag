using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayer : MonoBehaviour
{
    Vector3 move;
    public float speed = 4f;
    public float speedFactor = 1f;
    public CharacterController controller;
    public float skill1Delay = 5f;
    public float skill2Delay = 5f;

    void Start()
    {
        move = new Vector3(1f, 0f, 0f);
        Debug.Log(speedFactor);
    }

    // Update is called once per frame
    void Update()
    {        
        controller.Move(move * speed * speedFactor * Time.deltaTime);
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Skill1") {
            Skill1();
        }

        if (col.gameObject.tag == "Skill2") {
            Skill2();
        }
    }
    
    // HIT BY SKILL1 : SLOW
    public void Skill1()
    {
        speedFactor = 0.5f;
        StartCoroutine(DoResetSkill1Factor(skill1Delay));
    }

    IEnumerator DoResetSkill1Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        speedFactor = 1f;
    }

    // HIT BT SKILL2 : FREEZE    
    public void Skill2()
    {
        speedFactor = 0f;
        StartCoroutine(DoResetSkill2Factor(skill2Delay));
    }

    IEnumerator DoResetSkill2Factor(float delay)
    {
        yield return new WaitForSeconds(delay);
        speedFactor = 1f;
    }
}
