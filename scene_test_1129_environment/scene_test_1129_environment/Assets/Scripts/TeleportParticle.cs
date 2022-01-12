using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportParticle : MonoBehaviour
{
    public GameObject point;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = point.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
