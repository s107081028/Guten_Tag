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

    //Custom properites of player
    private string playerCharacterSelectNum = "CharacterNum";

    //UI data
    private bool leftIsGhost;

    //UI object
    private GameObject leftNameGhost;
    private GameObject rightNameGhost;
    private Button startButton;
    private Button switchButton;


    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    private void Start()
    {
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

        FindOpponent();
    }

    public void FindOpponent()
    {
        isConnecting = true;

        //findOpponentPanel.SetActive(false);
        waitingStatusText.text = "Searching...";


        if (PhotonNetwork.IsConnected)
        {
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
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomProperties = new Hashtable() { { ghostKey, true } };
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreatedRoom()
    {

        Debug.Log("Created A Room");
        leftName.text = PhotonNetwork.NickName;
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
            
            
            rightName.gameObject.SetActive(true);
            rightName.text = PhotonNetwork.NickName;
            leftName.text = PhotonNetwork.MasterClient.NickName;
            rightCharacter.SetActive(true);

            rightSkillBox.SetActive(true);
            rightButtons.SetActive(true);
            switchButton.interactable = true;


            // Fix bug
            //leftSkillBox.GetComponent<SkillBoxChanger>().callTheOtherPlayerSkin();
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


            leftSkillBox.GetComponent<SkillBoxChanger>().updateSkin();
        }
        
    }




    public void StartGame()
    {
        startButton.interactable = false;
        Invoke(nameof(EnableStartButton), 1);
        PhotonNetwork.LoadLevel("Environment");

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
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //todo show other player gone on UI
    }



}