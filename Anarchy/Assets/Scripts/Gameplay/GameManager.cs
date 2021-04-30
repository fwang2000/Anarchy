using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    private PlayerSpawner spawner;
    [SerializeField] private GameObject spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerControl.LocalPlayerInstance == null)
        {
            spawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
            spawner.InstantiatePlayer();
        }

        PlayerControl.LocalPlayerInstance.transform.position = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
    }
}
