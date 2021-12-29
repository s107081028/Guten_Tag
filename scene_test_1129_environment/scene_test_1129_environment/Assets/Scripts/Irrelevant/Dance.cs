using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dance : MonoBehaviourPun
{
    private Animator m_animator;
    bool dancing;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        dancing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (dancing)
            {
                m_animator.SetTrigger("UnDance");
            }
            else
            {
                m_animator.SetTrigger("Dance");
            }
            photonView.RPC(nameof(DanceSelf), RpcTarget.Others);
            dancing = !dancing;
        }
    }

    [PunRPC]
    void DanceSelf()
    {
        if (!photonView.IsMine) return;
        if (dancing)
        {
            m_animator.SetTrigger("UnDance");
        }
        else
        {
            m_animator.SetTrigger("Dance");
        }
        dancing = !dancing;
    }
}
