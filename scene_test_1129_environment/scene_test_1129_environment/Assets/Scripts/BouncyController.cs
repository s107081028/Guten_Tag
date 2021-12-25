using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Collision)
    {
        //if (Collision.gameObject.tag != "Plane")
        if (Collision.gameObject.tag == "Player")
        {
            Collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Collision.gameObject.GetComponent<Rigidbody>().velocity.x, 0, Collision.gameObject.GetComponent<Rigidbody>().velocity.z);
            Collision.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0, speed, 0);
        }
    }
}
