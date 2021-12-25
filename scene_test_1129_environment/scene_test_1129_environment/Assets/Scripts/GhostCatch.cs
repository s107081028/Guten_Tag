using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PhotonTutorial;

public class GhostCatch : MonoBehaviourPun
{
    public PlayerSpawner playerspawner = null;
    bool tagging;
    float cnt;
    // Start is called before the first frame update
    void Start()
    {
        playerspawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>();
        tagging = false;
        cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && (!tagging))
        {
            tagging = true;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (tagging)
        {
            if (cnt <= 10) // signal last (and cool down) for 10 frame
            {
                cnt++;
            }
            else
            {
                cnt = 0;
                tagging = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Debug.Log(other.name);
        if(tagging && other.tag == "Player")
        {
            // end game
            playerspawner.CallGhostWin();
        }
    }
}
