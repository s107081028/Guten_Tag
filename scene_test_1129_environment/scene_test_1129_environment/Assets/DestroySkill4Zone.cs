using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySkill4Zone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);     // SKILL4 ZONE DELAY
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.gameObject.tag == "Player") {
    //         if (photonView.IsMine) {
    //             Debug.Log("Skill4 Buff");
    //         }
    //         else {
    //             Debug.Log("Skill4 Debuff");
    //         }
    //     }
    // }
}
