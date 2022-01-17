using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OngroundTest : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Onground;
    void Start()
    {
        Onground = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag!="Player")
            Onground = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
            Onground = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            Onground = false;
    }
    
}
