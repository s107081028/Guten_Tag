using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuStart : MonoBehaviourPunCallbacks
{
    //[SerializeField] private GameObject findOpponentPanel = null;
    [SerializeField] private GameObject waitingStatusPanel = null;
    [SerializeField] private TextMeshProUGUI waitingStatusText = null;
    [SerializeField] private TextMeshProUGUI leftName = null;
    [SerializeField] private TextMeshProUGUI rightName = null;
    [SerializeField] private GameObject leftCharacter = null;
    [SerializeField] private GameObject rightCharacter = null;
    [SerializeField] private GameObject leftButtons = null;
    [SerializeField] private GameObject rightButtons = null;
    [SerializeField] private GameObject leftSkillBox = null;
    [SerializeField] private GameObject rightSkillBox = null;

    public GameObject menuPlayer;

    private bool isConnecting = false;

    private const string GameVersion = "0.1";
    private const int MaxPlayersPerRoom = 2;


    //Custom properites of per room
    private string ghostKey = "Ghost";
    private string mapKey = "Map";

    //Custom properites of player
    private string playerCharacterSelectNum = "CharacterNum";

    //UI data
    private bool leftIsGhost;

    //UI object
    private GameObject leftNameGhost;
    private GameObject rightNameGhost;
    private Button startButton;
    private Button switchButton;



    //Map data
    private int currentMapNum = 0;


    //Map UI

    [Header("Map")]

    public Image mapImg;
    public TextMeshProUGUI mapText;
    public List<Button> MapButtons;
    public List<Sprite> mapImgList;
    public List<string> mapNameList;

    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        switchButton = GameObject.Find("SwitchGhostButton").GetComponent<Button>();
        leftNameGhost = leftName.gameObject.transform.Find("GhostImg").gameObject;
        rightNameGhost = rightName.gameObject.transform.Find("GhostImg").gameObject;
        waitingStatusPanel.SetActive(true);
        leftName.gameObject.SetActive(false);
        rightName.gameObject.SetActive(false);
        leftCharacter.SetActive(false);
        rightCharacter.SetActive(false);
        rightButtons.SetActive(false);
        leftButtons.SetActive(false);
        leftSkillBox.SetActive(false);
        rightSkillBox.SetActive(false);

        //Default Left(Master if ghost)
        rightNameGhost.SetActive(false);

        if (PhotonNetwork.InRoom)
        {
            UpdateCurrentState();
        }
        else
        {
            FindOpponent();
        }
    }

    public void FindOpponent()
    {
        isConnecting = true;

        //findOpponentPanel.SetActive(false);
        waitingStatusText.text = "Searching...";


        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        //findOpponentPanel.SetActive(true);

        Debug.Log($"Disconnected due to: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new Hashtable() { { ghostKey, true },{ mapKey,0} };
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreatedRoom()
    {

        Debug.Log("Created A Room");
        leftName.text = PhotonNetwork.NickName;
        setupMap();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully joined a room");




        Hashtable playerProp = PhotonNetwork.LocalPlayer.CustomProperties;
        playerProp.Add(playerCharacterSelectNum, 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProp);


        leftCharacter.SetActive(true);
        leftSkillBox.SetActive(true);
        leftName.gameObject.SetActive(true);


        UpdateMapUI();

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount != MaxPlayersPerRoom)
        {
            
            waitingStatusText.text = "Waiting For Opponent";
            Debug.Log("Client is waiting for an opponent");
            
           
            leftName.text = PhotonNetwork.NickName;

            leftButtons.SetActive(true);
        }
        else
        {
            DisableButtonForMaster();
            waitingStatusText.text = "Opponent Found";
            Debug.Log("Match is ready to begin");
            Room room = PhotonNetwork.CurrentRoom;
            Hashtable roomProperties = room.CustomProperties;
            currentMapNum = (int)roomProperties[mapKey];


            rightName.gameObject.SetActive(true);
            rightName.text = PhotonNetwork.NickName;
            leftName.text = PhotonNetwork.MasterClient.NickName;
            rightCharacter.SetActive(true);

            rightSkillBox.SetActive(true);
            rightButtons.SetActive(true);
            switchButton.interactable = true;
            
            UpdateMapUI();
        }
        waitingStatusPanel.SetActive(false);

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {

            
            PhotonNetwork.CurrentRoom.IsOpen = false;
            rightName.gameObject.SetActive(true);
            rightCharacter.SetActive(true);
            rightSkillBox.SetActive(true);
            rightName.text = newPlayer.NickName;

            waitingStatusText.text = "Opponent Found";
            Debug.Log("Match is ready to begin");
            switchButton.interactable = true;



            // Fix bug
            //leftSkillBox.GetComponent<SkillBoxChanger>().updateSkin();
            leftSkillBox.GetComponent<PhotonView>().RPC("updateSkinRPC", RpcTarget.All, leftSkillBox.GetComponent<SkillBoxChanger>().curSkillNum);
        }
        
    }




    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        startButton.interactable = false;
        Invoke(nameof(EnableStartButton), 1);

        if (currentMapNum == 0)
            PhotonNetwork.LoadLevel("Environment");
        else {
            PhotonNetwork.LoadLevel("Environment(map2)"); //change this
        }
    }

    private void EnableStartButton() {
        startButton.interactable = true;
    }

    public void SwitchGhost()
    {

        Room room = PhotonNetwork.CurrentRoom;
        Hashtable roomProperties = room.CustomProperties;
        roomProperties[ghostKey] = !(bool)roomProperties[ghostKey];
        room.SetCustomProperties(roomProperties);
        Debug.Log("Swtich ghost : " + (bool)roomProperties[ghostKey]);

        //bool leftIsGhost = (bool)PhotonNetwork.CurrentRoom.CustomProperties["Ghost"];
        //room.SetCustomProperties()
    }


    void setupMap() {
        Room room = PhotonNetwork.CurrentRoom;
        Hashtable roomProperties = room.CustomProperties;
        roomProperties[mapKey] = 0;
        room.SetCustomProperties(roomProperties);
    }

    public void ChangeMap(int mapNumber)
    {

        Room room = PhotonNetwork.CurrentRoom;
        Hashtable roomProperties = room.CustomProperties;
        roomProperties[mapKey] = mapNumber;
        room.SetCustomProperties(roomProperties);
        Debug.Log("ChangeMap : " + (int)roomProperties[mapKey]);

        UpdateMapUI();

    }


    [PunRPC]
    private void ChangeNextMap()
    {
        currentMapNum++;
        currentMapNum %= mapImgList.Count;

        ChangeMap(currentMapNum);
    }

    [PunRPC]
    private void ChangePreMap() {
        currentMapNum--;
        currentMapNum += mapImgList.Count;
        currentMapNum %= mapImgList.Count;
        ChangeMap(currentMapNum);
    }

    public void ChangeNextMapRPC() {
        photonView.RPC(nameof(ChangeNextMap),RpcTarget.All);
    }

    public void ChangePreMapRPC()
    {
        photonView.RPC(nameof(ChangePreMap), RpcTarget.All);
    }

    private void UpdateMapUI() {
        Debug.Log("update map ui" + currentMapNum);
        mapImg.sprite = mapImgList[currentMapNum];
        mapText.text = mapNameList[currentMapNum];
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {


        //base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("onRoom properites changed");
        if (propertiesThatChanged.ContainsKey(ghostKey))
        {
            leftIsGhost = (bool)propertiesThatChanged[ghostKey];
            Debug.Log("OnRoomProperitesChanged : ghost" + leftIsGhost);
            SwtcihGhostUI();
        }
    }


    // UI function

    public void SwtcihGhostUI()
    {

        if (leftIsGhost)
        {

            leftNameGhost.gameObject.SetActive(true);
            rightNameGhost.gameObject.SetActive(false);
            return;
        }

        leftNameGhost.gameObject.SetActive(false);
        rightNameGhost.gameObject.SetActive(true);
    }


    public void UpdatePlayerCharacter(int num) {
        Debug.Log("update player character to " + num);
        Hashtable playerProp = PhotonNetwork.LocalPlayer.CustomProperties;
        playerProp[playerCharacterSelectNum] = num;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProp);
    }

    private void DisableButtonForMaster() {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            return;
        startButton.interactable = false;
        foreach (Button button in MapButtons) {
            button.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //todo show other player gone on UI
    }



    // shaoting



    public void UpdateCurrentState()
    {
        //Hashtable playerProp = PhotonNetwork.LocalPlayer.CustomProperties;
        //playerProp.Add(playerCharacterSelectNum, 0);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(playerProp);


        leftCharacter.SetActive(true);
        leftSkillBox.SetActive(true);
        leftName.gameObject.SetActive(true);

        //      setupMap();
        Room room = PhotonNetwork.CurrentRoom;
        Hashtable roomProperties = room.CustomProperties;
        currentMapNum = (int)roomProperties[mapKey];

        UpdateMapUI();

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount != MaxPlayersPerRoom)
        {

            //waitingStatusText.text = "Waiting For Opponent";
            //Debug.Log("Client is waiting for an opponent");


            leftName.text = PhotonNetwork.NickName;

            leftButtons.SetActive(true);
            //leftSkillBox.GetComponent<SkillBoxChanger>().updateSkin();
        }
        else
        {
            DisableButtonForMaster();
            rightName.gameObject.SetActive(true);
            rightCharacter.SetActive(true);
            rightSkillBox.SetActive(true);
            switchButton.interactable = true;


            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player otherPlayer = null;
                foreach (Player player in PhotonNetwork.PlayerList) {
                    if (player.UserId != PhotonNetwork.LocalPlayer.UserId)
                    {
                        otherPlayer = player;
                        break;
                    }
                }


                rightName.text = otherPlayer.NickName;
                leftName.text = PhotonNetwork.MasterClient.NickName;
                

                leftButtons.SetActive(true);

                //rightSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)otherPlayer.CustomProperties["CharacterNum"]);
                //Invoke(nameof(UpdateSkinForOther), 1);
                //leftSkillBox.GetComponent<SkillBoxChanger>().updateSkin();
                //leftSkillBox.GetComponent<PhotonView>().RPC("updateSkinRPC", RpcTarget.All, (int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"]);
            }
            else
            {
                leftName.text = PhotonNetwork.MasterClient.NickName;
                rightName.text = PhotonNetwork.LocalPlayer.NickName;


                rightButtons.SetActive(true);


                //Invoke(nameof(UpdateSkinForOther), 1);
                //rightSkillBox.GetComponent<SkillBoxChanger>().updateSkin();
                //rightSkillBox.GetComponent<PhotonView>().RPC("updateSkinRPC", RpcTarget.All, rightSkillBox.GetComponent<SkillBoxChanger>().curSkillNum);
                //rightSkillBox.GetComponent<PhotonView>().RPC("updateSkinRPC", RpcTarget.All, (int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"]);
               // rightSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"]);

            }

            //leftSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)PhotonNetwork.MasterClient.CustomProperties["CharacterNum"]);

        }
        Invoke(nameof(UpdateSkinForOther), 0.1f);
        //UpdateSkinForOther();
        waitingStatusPanel.SetActive(false);


    }

    private void UpdateSkinForOther()
    {
        Player otherPlayer = null;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.UserId != PhotonNetwork.LocalPlayer.UserId)
            {
                otherPlayer = player;
                break;
            }
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(otherPlayer!= null)
                rightSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)otherPlayer.CustomProperties["CharacterNum"]);
        }
        else
        {
            rightSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterNum"]);
        }
        leftSkillBox.GetComponent<SkillBoxChanger>().updateSkinRPC((int)PhotonNetwork.MasterClient.CustomProperties["CharacterNum"]);
    }



}