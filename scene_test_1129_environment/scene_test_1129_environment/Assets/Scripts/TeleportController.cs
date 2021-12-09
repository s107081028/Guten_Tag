using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject AnotherTeleport;
    public float lastEnterTime;
    public float coolTime;
    // Start is called before the first frame update
    void Start()
    {
        lastEnterTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision Collision)
    {
        print(Time.time - lastEnterTime);
        if(Collision.gameObject.tag == "Player" && (Time.time - lastEnterTime) >= coolTime)
        {
            AnotherTeleport.GetComponent<TeleportController>().lastEnterTime = Time.time;
            lastEnterTime = Time.time;
            Collision.transform.position = AnotherTeleport.transform.position;
        }
    }

}
