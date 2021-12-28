using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ParticleSpin : MonoBehaviourPun
{
    [SerializeField] Vector3 speed;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
