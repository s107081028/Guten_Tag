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
    private TestOutline outline;

    private GhostCatch catchComponent;
    private HumanDash dashComponent;

    public bool isCloned = false;
    // Set Name and set ghost grab
    void Start()
    {
        catchComponent = GetComponent<GhostCatch>();
        dashComponent = GetComponent<HumanDash>();
        outline = GetComponent<TestOutline>();
        //if (photonView.IsMine) return;
        SetName();
        SetGhost();
        SetSelfOutline();
        
    }
    private void SetName() {
        nameText.text = photonView.Owner.NickName;
    }

    private void SetGhost() {
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["Ghost"])
        {
            if (photonView.Owner.IsMasterClient) {
                TurnOfGhostObject();
            }
            if (!photonView.Owner.IsMasterClient) {
                TurnOfHumanObject();
            }
        }
        else {
            if (!photonView.Owner.IsMasterClient) {
                TurnOfGhostObject();
            }
            if (photonView.Owner.IsMasterClient) {
                TurnOfHumanObject();
            }
        }
    }

    private void TurnOfGhostObject() {

        ghostIcon.SetActive(true);

        if (!isCloned) {
            ghostGrab.SetActive(true);
            catchComponent.enabled = true;
        }


        if (photonView.IsMine) return;

        //only human see red outline of ghost
        outline.turnOnOutline(Color.red);
        

    }

    private void TurnOfHumanObject() {
        if (!isCloned) {
            dashComponent.enabled = true;
        }
        
    }

    private void SetSelfOutline() {
        // everyone see itself white outline
        if (photonView.IsMine) {
            outline.turnOnOutline(Color.white);
            
           // GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll
        }
    }


    


}
