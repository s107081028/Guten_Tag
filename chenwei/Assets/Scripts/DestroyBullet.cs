using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
