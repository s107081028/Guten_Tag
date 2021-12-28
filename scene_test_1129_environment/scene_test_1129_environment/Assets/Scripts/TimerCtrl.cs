using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PhotonTutorial;

public class TimerCtrl : MonoBehaviourPun
{

    public float timecntdown = 60;
    int time_int;
    public Text timeText;
    PhotonView PV;
    public PlayerSpawner playerspawner = null;


    private List<GameObject> energyBars;

    // Start is called before the first frame update
    void Start()
    {
        playerspawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>();
        PV = GetComponent<PhotonView>();
        timeText = GetComponent<Text>();
        time_int = Mathf.FloorToInt(timecntdown);



        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PV.RPC("showText", RpcTarget.All, timecntdown);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (timecntdown > 0)
        {
            timecntdown -= Time.deltaTime;
        }else
        {
            timecntdown = 0;
            // end game
            playerspawner.CallHumanWin();
        }

        if(time_int!= Mathf.FloorToInt(timecntdown))
        {
            time_int = Mathf.FloorToInt(timecntdown);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PV.RPC("showText", RpcTarget.All, timecntdown);
            }
        }
        else
        {
            showText(timecntdown);
        }

    }

    [PunRPC]
    void showText(float timeValue)
    {
        if (timeValue < 0)
        {
            timeValue = 0;
        }
        timecntdown = timeValue;
        float min = Mathf.FloorToInt(timeValue / 60);
        float sec = Mathf.FloorToInt(timeValue % 60);

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }


}
