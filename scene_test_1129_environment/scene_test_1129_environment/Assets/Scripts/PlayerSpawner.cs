using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PhotonTutorial
{
    public class PlayerSpawner : MonoBehaviourPun
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject playerPrefab2 = null;
        [SerializeField] private GameObject playerCamera = null;
        [SerializeField] private GameObject Conveyor1 = null;
        [SerializeField] private GameObject Conveyor2 = null;
        [SerializeField] private GameObject Teleport1 = null;
        [SerializeField] private GameObject Teleport2 = null;
        [SerializeField] private GameObject WinLoseCanvas = null;


        //Character List
        [SerializeField] private List<GameObject> characterList = new List<GameObject>();


        //Identify ghost player actor number
        private int ghostActorNum = 0;


        //ui data
        private string humanName = "";
        private string ghostName = "";

        //Game play ui
        private TextMeshProUGUI missionText;
        
        //win lose diaply ui
        private GameObject winUI;
        private GameObject loseUI;




        public Transform spawnPoint1;
        public Transform spawnPoint2;

        bool masterIsGhost = false;
        


        private void Start()
        {

            

            loseUI = WinLoseCanvas.transform.Find("Panel_LOSE").gameObject;
            winUI = WinLoseCanvas.transform.Find("Panel_WIN").gameObject;
            missionText = GameObject.Find("Mission_text").GetComponent<TextMeshProUGUI>();

            setGhostAndHuman();

            setMissionText();

            //var player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-20f, 10f), 1f, Random.Range(5f, 10f)), Quaternion.identity);
            if (PhotonNetwork.LocalPlayer.IsMasterClient) {

                spawnPlayerByCharacterNum((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"],spawnPoint1.position);
                //var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint1.position, Quaternion.identity);
            }else{
                spawnPlayerByCharacterNum((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"], spawnPoint2.position);
            }
            
            
            
            
            //playerCamera = GameObject.Find("Main Camera");
            //playerCamera.GetComponent<Transform>().position = new Vector3(player.transform.position.x, 6.5f, player.transform.position.z) 
            //- player.transform.forward*10f;
            //playerCamera.GetComponent<Transform>().LookAt(player.transform);
            // var conveyor1 = PhotonNetwork.Instantiate(Conveyor1.name, new Vector3(Conveyor1.GetComponent<ConveyorController>().x1, Conveyor1.GetComponent<ConveyorController>().y1, Conveyor1.GetComponent<ConveyorController>().z1), Quaternion.identity);
            // var conveyor2 = PhotonNetwork.Instantiate(Conveyor2.name, new Vector3(Conveyor2.GetComponent<ConveyorController>().x1, Conveyor2.GetComponent<ConveyorController>().y1, Conveyor2.GetComponent<ConveyorController>().z1), Quaternion.identity);
            // var teleport1 = PhotonNetwork.Instantiate(Teleport1.name, new Vector3(15.99f, 0.25f, 41f), Quaternion.identity);
            // var teleport2 = PhotonNetwork.Instantiate(Teleport2.name, new Vector3(-22.26f, 0.05f,-16.4f), Quaternion.identity);
        }

        public void CallGhostWin()
        {
            photonView.RPC("GhostWin", RpcTarget.All);
        }

        public void CallHumanWin()
        {
            photonView.RPC("HumanWin", RpcTarget.All);
        }

        [PunRPC]
        public void GhostWin()
        {

            WinLoseCanvas.SetActive(true);


            //if this player is ghost
            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                showWinUI();

            }
            else {
                showLoseUI();
            }


            /**

            if(PhotonNetwork.LocalPlayer.IsMasterClient && masterIsGhost)
            {
                //show win
                WinLoseCanvas.transform.Find("LOSEText").gameObject.SetActive(false);
                Debug.Log("show win");
            }
            else if((!PhotonNetwork.LocalPlayer.IsMasterClient) && (!masterIsGhost))
            {
                //show win
                WinLoseCanvas.transform.Find("LOSEText").gameObject.SetActive(false);
            }
            else
            {
                //show lose
                
            }*/
        }

        void showWinUI() {
            Debug.Log("You win!!!!!");
            loseUI.SetActive(false);
            winUI.SetActive(true);
        }

        void showLoseUI()
        {
            Debug.Log("You lose!!!");
            loseUI.SetActive(true);
            winUI.SetActive(false);
        }

        [PunRPC]
        public void HumanWin()
        {

            WinLoseCanvas.SetActive(true);

            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                showLoseUI();

            }
            else {
                showWinUI();
            }

            /*

            if (PhotonNetwork.LocalPlayer.IsMasterClient && masterIsGhost)
            {
                //show lose
                WinLoseCanvas.transform.Find("WINText").gameObject.SetActive(false);
            }
            else if ((!PhotonNetwork.LocalPlayer.IsMasterClient) && (!masterIsGhost))
            {
                //show lose
                WinLoseCanvas.transform.Find("WINText").gameObject.SetActive(false);
            }
            else
            {
                //show win
                WinLoseCanvas.transform.Find("LOSEText").gameObject.SetActive(false);
            }*/
        }

        public void restart()
        {
            //
            photonView.RPC("loadNextSync", RpcTarget.All);
        }

        [PunRPC]
        void loadNextSync()
        {
            PhotonNetwork.LoadLevel("Environment");
        }



        void spawnPlayerByCharacterNum(int num, Vector3 pos) {

            GameObject selectCharacter = characterList[num];
            PhotonNetwork.Instantiate(selectCharacter.name,pos, Quaternion.identity);

        }


        //set ghost actor number and their nick name
        void setGhostAndHuman() {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["Ghost"])
            {

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    ghostActorNum = PhotonNetwork.LocalPlayer.ActorNumber;


                }
                else
                {
                    ghostActorNum = PhotonNetwork.PlayerListOthers[0].ActorNumber;
                }

                
            }
            else
            {
                humanName = PhotonNetwork.MasterClient.NickName;
                if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    ghostActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

                }
                else
                {
                    ghostActorNum = PhotonNetwork.PlayerListOthers[0].ActorNumber;
                }


            }

            //set name
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList) {
                if (player.ActorNumber == ghostActorNum)
                {
                    ghostName = player.NickName;
                }
                else {
                    humanName = player.NickName;
                }
            }
        }

        void setMissionText() {
            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                missionText.text = "Catch " + humanName + "!";
            }
            else {
                missionText.text = "Don't get caught!";
            }
        }
    }

}