using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNetworkManager : MonoBehaviourPun
{


    //For network related function in Room scene
    //1. Change character
    //2. Switch Ghost
    public GameObject menuPlayer;

    private void Start()
    {
       // PhotonNetwork.Instantiate(menuPlayer.name, Vector3.zero, Quaternion.identity);
    }


}
