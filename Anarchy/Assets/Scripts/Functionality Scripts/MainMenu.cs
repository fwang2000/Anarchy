using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [Header("Top Panel")]
    public GameObject topPanel;

    [Header("Menu Panel")]
    public GameObject menuPanel;

    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public InputField RoomNameInputField;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;

    /*
    [SerializeField]
    private GameObject findOpponentPanel = null;

    [SerializeField]
    private GameObject waitingStatusPanel = null;

    [SerializeField]
    private TextMeshProUGUI waitingStatusText = null;

    private bool isConnecting = false; */

    private const string GameVersion = "0.1"; // if we update the game, we want to ensure that players with different game versions cannot play each other

    private const int MaxPlayersPerRoom = 3; // reset to 8 laters

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

    private PlayerSpawner spawner;

    #region UNITY

    public void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        spawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();

        PlayerNameInput.placeholder.GetComponent<Text>().text = "Enter Player Name";
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        this.SetActivePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(SelectionPanel.name);

        Debug.Log($"Disconnected due to: {cause}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room");

        string roomName = "Room " + Random.Range(1000, 10000);

        CreateRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        cachedRoomList.Clear();

        Debug.Log("Client successfully joined a room");

        topPanel.SetActive(false);
        SetActivePanel(InsideRoomPanel.name);
    }   
    #endregion

    #region UI CALLBACKS

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + UnityEngine.Random.Range(1000, 10000) : roomName;

        CreateRoom(roomName);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(JoinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLoginButtonClicked()
    {

        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PlayerNameInput.placeholder.GetComponent<Text>().text = "Can't be empty!";
        }
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(RoomListPanel.name);
    }

    public void OnPlayerButtonClicked()
    {
        menuPanel.GetComponent<Canvas>().enabled = false;

        GameObject currButtonGameObject = EventSystem.current.currentSelectedGameObject;
        Button currButton = currButtonGameObject.GetComponent<Button>();
        currButton.interactable = false;

        MakeNicknameUnique();
        spawner.InstantiatePlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " Entered");

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Max Players Reached");

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("MainGame");
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left");

        PhotonNetwork.DestroyPlayerObjects(otherPlayer);
    }

    #endregion

    #region HELPERS

    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
    }
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }

    private void MakeNicknameUnique()
    {
        int duplicate = 0;

        string[] nicknames = (string[]) PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

        while (nicknames.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            ++duplicate;
            PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.LocalPlayer.NickName + " (" + duplicate + ")";
        }

        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());

        nicknames[PhotonNetwork.LocalPlayer.GetPlayerNumber()] = PhotonNetwork.LocalPlayer.NickName;

        Hashtable setValue = new Hashtable();
        setValue.Add("nicknames", nicknames);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
    }

    private void CreateRoom(string roomName)
    {
        string[] nicknames = new string[8] { "", "", "", "", "", "", "", "" };
        
        PhotonNetwork.CreateRoom(roomName, new RoomOptions
        {
            MaxPlayers = MaxPlayersPerRoom,
            CustomRoomProperties = new Hashtable
                {
                    { "nicknames", nicknames }
                },
            PlayerTtl = 10000
        });
    }

    private void OnPlayerNumberingChanged()
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() >= 0)
        {
            MakeNicknameUnique();
            spawner.InstantiatePlayer();
        }
    }
    #endregion
}
