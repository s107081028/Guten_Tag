using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private GameObject GhostGPS = null;


        //Character List
        [SerializeField] private List<GameObject> characterList = new List<GameObject>();


        //Identify ghost player actor number
        private int ghostActorNum = 0;

        //Player GameObjects
        private GameObject player1;
        private GameObject player2;
        public List<GameObject> playerGameObjects;


        //ui data
        private string humanName = "";
        private string ghostName = "";

        //Game play ui
        private TextMeshProUGUI missionText;

        //win lose diaply ui
        private GameObject winUI;
        private GameObject loseUI;


        //timer
        public TimerCtrl timer;
        public List<GameObject> hideAfterWinLoseUI;

        private Button restartButton;


        public Transform spawnPoint1;
        public Transform spawnPoint2;

        bool masterIsGhost = false;


        private int currentMapNum;


        private void Start()
        {

            setMapNum();

            loseUI = WinLoseCanvas.transform.Find("Panel_LOSE").gameObject;
            winUI = WinLoseCanvas.transform.Find("Panel_WIN").gameObject;
            missionText = GameObject.Find("Mission_text").GetComponent<TextMeshProUGUI>();
            restartButton = WinLoseCanvas.transform.Find("Button").gameObject.GetComponent<Button>();

            setGhostAndHuman();

            setMissionText();


            //var player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-20f, 10f), 1f, Random.Range(5f, 10f)), Quaternion.identity);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {

                player1 = spawnPlayerByCharacterNum((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"], spawnPoint1.position);
                //var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint1.position, Quaternion.identity);
            }
            else
            {
                player2 = spawnPlayerByCharacterNum((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"], spawnPoint2.position);
            }

            playerGameObjects = new List<GameObject>() { player1, player2 };

            Invoke(nameof(setGhostGPS), 2);




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

        void resetCursor() {
            Cursor.lockState = CursorLockMode.None;
        }

        void setMapNum() {
            Room room = PhotonNetwork.CurrentRoom;
            Hashtable roomProperties = room.CustomProperties;

            currentMapNum = (int)roomProperties["Map"];
        }

        [PunRPC]
        public void GhostWin()
        {
            timer.enabled = false;
            WinLoseCanvas.SetActive(true);
            Invoke(nameof(stopPlayerControll), 0.2f);
            resetCursor();

            //if this player is ghost
            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                showWinUI();

            }
            else
            {
                showLoseUI();
            }


        }

        void showWinUI()
        {
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

        void stopPlayerControll()
        {
            Debug.Log("STOP PLAYER CONTROLL");

            GameObject curPlayer = (player1) ? player1 : player2;
            curPlayer.GetComponent<Animator>().enabled = false;
            curPlayer.GetComponent<PlayerController>().enabled = false;
            curPlayer.GetComponent<SkillController>().enabled = false;
            curPlayer.GetComponent<GhostCatch>().enabled = false;
            curPlayer.GetComponent<HumanDash>().enabled = false;


            //hide gameplay ui

            hideUI();

        }

        [PunRPC]
        public void HumanWin()
        {
            timer.enabled = false;
            WinLoseCanvas.SetActive(true);
            Invoke(nameof(stopPlayerControll), 0.2f);
            resetCursor();

            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                showLoseUI();
            }
            else
            {
                showWinUI();

            }
        }

        public void restart()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                loadNextSync();
            }
            //photonView.RPC("loadNextSync", RpcTarget.All);
        }

        [PunRPC]
        void loadNextSync()
        {
            PhotonNetwork.LoadLevel("RoomScene");
            /*
            if (currentMapNum == 0)
                PhotonNetwork.LoadLevel("Environment");
            else
            {
                PhotonNetwork.LoadLevel("Environment(map2)"); //change this
            }
            */
        }



        GameObject spawnPlayerByCharacterNum(int num, Vector3 pos)
        {

            GameObject selectCharacter = characterList[num];
            return PhotonNetwork.Instantiate(selectCharacter.name, pos, Quaternion.identity);

        }


        //set ghost actor number and their nick name
        void setGhostAndHuman()
        {
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
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == ghostActorNum)
                {
                    ghostName = player.NickName;
                }
                else
                {
                    humanName = player.NickName;
                }
            }
        }

        void setMissionText()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == ghostActorNum)
            {
                missionText.text = "Catch " + humanName + "!";
            }
            else
            {
                missionText.text = "Don't get caught!";
            }
        }

        void setGhostGPS()
        {
            //set human can see ghost gps
            if (PhotonNetwork.LocalPlayer.ActorNumber != ghostActorNum)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        print(player.name);
                        GhostGPS.GetComponent<TestScreenPoint>().player = player.transform;
                    }
                    else
                    {
                        print(player.name);
                        GhostGPS.GetComponent<TestScreenPoint>().target = player.transform;
                    }
                }


                GhostGPS.SetActive(true);
            }
        }



        void hideUI()
        {
            if (!PhotonNetwork.IsMasterClient)
                restartButton.interactable = false;

            foreach (GameObject obj in hideAfterWinLoseUI)
            {
                obj.SetActive(false);
            }

        }
    }




}