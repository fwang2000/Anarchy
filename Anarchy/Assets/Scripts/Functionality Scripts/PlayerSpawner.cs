﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using System;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spawnpoints;

    private static GameObject player;
    private Vector3 offset;
    private Transform cameraTransform;

    public void InstantiatePlayer()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position, Quaternion.identity);
        cameraTransform = Camera.main.GetComponent<Transform>();
        offset = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform != null && player != null)
        {
            cameraTransform.position = player.transform.position + offset;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
