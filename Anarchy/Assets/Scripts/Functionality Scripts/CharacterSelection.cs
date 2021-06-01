using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.EventSystems;

public class CharacterSelection : MonoBehaviourPunCallbacks
{
    private PlayerSpawner spawner;
    private GameObject playerCharacter;
    private Hashtable playerProps = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
    }

    public void OnPlayerButtonClicked()
    {
        string oldCharacterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];

        if (oldCharacterName != null)
        {
            GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }
        else
        {
            MakeNicknameUnique();
            spawner.InstantiatePlayer();
            playerCharacter = spawner.GetPlayer();
            playerCharacter.GetComponent<PlayerControl>().SetID(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        string newCharacterName = EventSystem.current.currentSelectedGameObject.name;
        GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.AllBuffered, newCharacterName);

        SetCustomCharacter(newCharacterName);

        transform.gameObject.SetActive(false);

        GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBuffered);
    }

    private void MakeNicknameUnique()
    {
        int duplicate = 0;

        string[] nicknames = (string[]) PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

        while (nicknames.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            ++duplicate;
            if (duplicate == 1)
            {
                PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.LocalPlayer.NickName + " (" + duplicate + ")";
            }
            else
            {
                string currNickname = PhotonNetwork.LocalPlayer.NickName;
                PhotonNetwork.LocalPlayer.NickName = currNickname.Substring(0, currNickname.Length - 4) + " (" + duplicate + ")";
            }
        }

        nicknames[PhotonNetwork.LocalPlayer.GetPlayerNumber()] = PhotonNetwork.LocalPlayer.NickName;

        Hashtable setValue = new Hashtable();
        setValue.Add("nicknames", nicknames);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
    }

    [PunRPC]
    private void DisableButton(string name)
    {
        Button currButton = GetCharacterButton(name);
        currButton.interactable = false;
    }

    [PunRPC]
    private void ActivateButton(string name)
    {
        Button currButton = GetCharacterButton(name);
        currButton.interactable = true;
    }

    [PunRPC]
    private void SetColor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.CustomProperties["character"]);
        }

        foreach (GameObject playerModel in players)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                Debug.Log(playerModel.GetComponent<PlayerControl>().GetPlayerID() + ", " + player.ActorNumber);
                if (playerModel.GetComponent<PlayerControl>().GetPlayerID() == player.ActorNumber)
                {
                    string color = (string) player.CustomProperties["character"];
                    playerModel.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + color);
                    
                }
            }
        }
    }

    private Button GetCharacterButton(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject.GetComponent<Button>();
            }
        }
        Debug.LogError("cannot find button - returned null");
        return null;
    }

    private void SetCustomCharacter(string newCharacterName)
    {
        string character = newCharacterName;

        playerProps["character"] = character;
        PhotonNetwork.LocalPlayer.CustomProperties = playerProps;
    }
}
