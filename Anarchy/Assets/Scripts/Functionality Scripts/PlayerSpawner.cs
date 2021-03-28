using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    private GameObject player;
    private Vector3 offset;
    private Transform cameraTransform;

    [SerializeField]
    private GameObject spawnpoints;

    private Player[] allPlayers;
    private int myNumberInRoom;

    // Start is called before the first frame update
    void Start()
    {
        allPlayers = PhotonNetwork.PlayerList;
        foreach(Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumberInRoom++;
            }
        }

        Debug.Log(myNumberInRoom);

        player = PhotonNetwork.Instantiate(playerPrefab.name, SpawnpointController.singletonInstance.spawnpoints[myNumberInRoom].position, Quaternion.identity);
        cameraTransform = Camera.main.GetComponent<Transform>();
        offset = cameraTransform.position;
    }

    void LateUpdate()
    {
        cameraTransform.position = player.transform.position + offset;
    }
}
