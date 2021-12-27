using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotateSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(h) >= 0.01) {
            transform.Rotate(0f,h*rotateSpeed*Time.deltaTime,0f);

        }
        if (Mathf.Abs(v) >= 0.01) {

            transform.Translate(transform.forward * speed * Time.deltaTime);
        }


    }
}
