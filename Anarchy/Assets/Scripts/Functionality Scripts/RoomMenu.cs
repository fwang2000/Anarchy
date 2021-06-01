using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.EventSystems;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [Header("Character Select")]
    public GameObject CharacterSelect;

    private PlayerSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
    }

    public void OnPlayerButtonClicked()
    {
        string oldCharacterName = (string) PhotonNetwork.LocalPlayer.CustomProperties["character"];

        if (oldCharacterName != null)
        {
            Debug.Log("Here");
            GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }
        else
        {
            MakeNicknameUnique();
            spawner.InstantiatePlayer();
        }

        string newCharacterName = EventSystem.current.currentSelectedGameObject.name;
        GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.AllBuffered, newCharacterName);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
            { "character", newCharacterName }
        });

        CharacterSelect.SetActive(false);
    }

    private void MakeNicknameUnique()
    {
        int duplicate = 0;

        string[] nicknames = (string[])PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

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
        GameObject currButtonGameObject = GameObject.Find("CharacterSelect/" + name);
        Button currButton = currButtonGameObject.GetComponent<Button>();
        currButton.interactable = false;

    }

    [PunRPC]
    private void ActivateButton(string name)
    {
        GameObject currButtonGameObject = GameObject.Find("CharacterSelect/" + name);
        Button currButton = currButtonGameObject.GetComponent<Button>();
        currButton.interactable = true;
    }

    [PunRPC]
    private void SetColor(Material playerMat, string color)
    {
        playerMat = Resources.Load("Materials/" + color, typeof(Material)) as Material;
    }
}
