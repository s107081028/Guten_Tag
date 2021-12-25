using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject ghostIcon;
    [SerializeField] private GameObject ghostGrab;

    private GhostCatch catchComponent;
    // Set Name and set ghost grab
    void Start()
    {
        catchComponent = GetComponent<GhostCatch>();
        //if (photonView.IsMine) return;
        SetName();
        SetGhost();
    }
    private void SetName() {
        nameText.text = photonView.Owner.NickName;
    }

    private void SetGhost() {
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["Ghost"])
        {
            if (photonView.Owner.IsMasterClient) {
                ghostIcon.SetActive(true);
                ghostGrab.SetActive(true);
                catchComponent.enabled = true;
            }
            
        }
        else {
            if (!photonView.Owner.IsMasterClient) {
                ghostIcon.SetActive(true);
                ghostGrab.SetActive(true);
                catchComponent.enabled = true;
            }
            
        }
    }
    


}
