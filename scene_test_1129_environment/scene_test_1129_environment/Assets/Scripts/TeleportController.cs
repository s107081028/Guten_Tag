using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject AnotherTeleport;
    public float lastEnterTime;
    public float coolTime;
    public Dictionary<string , float> dict = new Dictionary<string,  float>();
    // Start is called before the first frame update
    void Start()
    {
        lastEnterTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Collision)
    {   
        if(Collision.gameObject.tag == "Player")
        {
            if (!dict.ContainsKey(Collision.gameObject.name)) {
                dict.Add(Collision.gameObject.name, Time.time);
                AnotherTeleport.GetComponent<TeleportController>().dict.Add(Collision.gameObject.name, Time.time);
                Collision.transform.position = AnotherTeleport.transform.position;
            }
            else if (Time.time - dict[Collision.gameObject.name] >= coolTime)
            {
                dict[Collision.gameObject.name] = Time.time;
                AnotherTeleport.GetComponent<TeleportController>().dict[Collision.gameObject.name] = Time.time;
                Collision.transform.position = AnotherTeleport.transform.position;
            }      
        }
    }

}
