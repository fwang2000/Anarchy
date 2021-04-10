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

    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity);
        cameraTransform = Camera.main.GetComponent<Transform>();
        offset = cameraTransform.position;
    }

    void LateUpdate()
    {
        cameraTransform.position = player.transform.position + offset;
    }
}
