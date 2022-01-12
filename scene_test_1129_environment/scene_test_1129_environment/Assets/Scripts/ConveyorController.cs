using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ConveyorController : MonoBehaviourPun
{
    public float x1, y1, z1 ,x2, y2, z2;
    private float max, min;
    private Vector3 velocity;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.position = new Vector3(x1, y1, z1);
        velocity = new Vector3(x2-x1, y2-y1, z2-z1);
        if(z2 >= z1) {
            max = z2;
            min = z1;
        } else {
            max = z1;
            min = z2;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.transform.position.z > max || gameObject.transform.position.z < min)
        {
            velocity = -1 * velocity;
        }
        // gameObject.GetComponent<Rigidbody>().velocity = (speed * velocity);
        gameObject.transform.position += speed * velocity;
    }
    
    void OnCollisionStay(Collision CollisionObject)
    {
        if (!photonView.IsMine) return;
        if(CollisionObject.gameObject.tag == "Player")
        {
            CollisionObject.gameObject.GetComponent<Rigidbody>().velocity =
                CollisionObject.gameObject.GetComponent<Rigidbody>().velocity / 2;
            CollisionObject.gameObject.transform.position += speed * velocity;
            // CollisionObject.gameObject.GetComponent<Rigidbody>().velocity = CollisionObject.gameObject.GetComponent<Rigidbody>().velocity + speed * velocity;
        }
    }

}
