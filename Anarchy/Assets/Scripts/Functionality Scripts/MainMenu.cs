using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using System;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [Header("Top Panel")]
    public GameObject TopPanel;

    [Header("Menu Panel")]
    public GameObject MenuPanel;

    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public GameObject CharacterSelect;
    public GameObject StartGameButton;

    /*
    [SerializeField]
    private GameObject findOpponentPanel = null;

    [SerializeField]
    private GameObject waitingStatusPanel = null;

    [SerializeField]
    private TextMeshProUGUI waitingStatusText = null;

    private bool isConnecting = false; */

    private const string GameVersion = "0.1"; // if we update the game, we want to ensure that players with different game versions cannot play each other

    private const int MaxPlayersPerRoom = 3; // reset to 8 later

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

    // private PlayerSpawner spawner;

    #region UNITY

    public void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        // spawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();

        PlayerNameInput.placeholder.GetComponent<Text>().text = "Enter Player Name";

        TopPanel.SetActive(true);
        SetActivePanel(LoginPanel.name);
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

    public override void OnJoinedRoom()
    {
        cachedRoomList.Clear();

        Debug.Log("Client successfully joined a room");

        TopPanel.SetActive(false);
        SetActivePanel(InsideRoomPanel.name);
        CharacterSelect.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        TopPanel.SetActive(true);
        this.SetActivePanel(SelectionPanel.name);
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
        string roomName = "";
        roomName = (roomName.Equals(string.Empty)) ? "Room " + UnityEngine.Random.Range(1000, 10000) : roomName;

        CreateRoom(roomName);
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

    public void OnHomeButtonClicked()
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " Entered");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= MaxPlayersPerRoom - 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartGameButton.GetComponent<Button>().interactable = true;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Max Players Reached");

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left");
        ResetCharacterSelect((string)otherPlayer.CustomProperties["character"]);
        ResetNicknames(otherPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            InsideRoomPanel.transform.Find("RoomSettings").GetComponent<RoomSettings>().ResetMasterClient(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayersPerRoom - 2)
            {
                StartGameButton.GetComponent<Button>().interactable = false;
            }
            PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        }
    }

    #endregion

    #region HELPERS

    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
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

    private void CreateRoom(string roomName)
    {
        string[] nicknames = new string[8] { "", "", "", "", "", "", "", "" };
        int[] models = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

        PhotonNetwork.CreateRoom(roomName, new RoomOptions
        {
            MaxPlayers = MaxPlayersPerRoom,
            CustomRoomProperties = new Hashtable
                {
                    { "nicknames", nicknames },
                    { "models", models},
                    { "moveSpeed", 10f },
                    { "roundTime", 5f },
                    { "curseTime", 1f},
                    { "artifactsDR", "LOW"}
                },
            PlayerTtl = 0,
            EmptyRoomTtl = 0,
        });
    }

    private void ResetNicknames(Player player)
    {
        string[] nicknames = (string[])PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

        nicknames[player.GetPlayerNumber()] = "";

        Hashtable setValue = new Hashtable();
        setValue.Add("nicknames", nicknames);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
    }
    private void ResetCharacterSelect(string oldCharacterName)
    {
        Debug.Log(oldCharacterName);
        if (oldCharacterName != null)
        {
            CharacterSelect.GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }
    }

    public void OnRefreshButtonClicked()
    {
        ClearRoomListView();
        UpdateRoomListView();
    }
    #endregion
}
