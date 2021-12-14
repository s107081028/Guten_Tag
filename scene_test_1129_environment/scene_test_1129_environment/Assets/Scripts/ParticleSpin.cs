using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpin : MonoBehaviour
{
    [SerializeField] Vector3 speed;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
