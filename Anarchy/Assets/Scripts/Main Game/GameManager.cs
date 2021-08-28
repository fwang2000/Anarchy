using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private GameObject player;

    private PhotonView myPV;

    private int whichPlayerIsCursed;

    [SerializeField] private GameObject spawnpoints;
    [SerializeField] private GameObject curseTimerObject;

    private CurseTimer curseTimer;

    void Start()
    {
        ResetLocalPlayer();

        myPV = GetComponent<PhotonView>();
        curseTimer = curseTimerObject.GetComponent<CurseTimer>();

        if (PhotonNetwork.IsMasterClient)
        {
            myPV.RPC("SetGameSettings", RpcTarget.AllBuffered);
            CurseRandomPlayer();
            curseTimer.SetTime("StartCurseTime");
        }
    }

    public void PickCursed(int cursedPlayerNumber)
    {
        myPV.RPC("RPC_SyncCursed", RpcTarget.All, cursedPlayerNumber);
    }

    [PunRPC]
    void RPC_SyncCursed(int playerNumber)
    {
        whichPlayerIsCursed = playerNumber;
        PlayerControl.LocalPlayerInstance.BecomeCursed(playerNumber);
        curseTimer.SetTime("StartCurseTime");
    }

    [PunRPC]
    void SetGameSettings()
    {
        SetSpeed();
        SetTimer();
    }

    private void SetSpeed()
    {
        player.GetComponent<PlayerControl>().SetSpeed();
    }

    private void SetTimer()
    {
        // float time = (float)PhotonNetwork.CurrentRoom.CustomProperties["roundTime"];
        // timer.GetComponent<Timer>().SetTime(time);
    }

    private void SetCurseTimer()
    {
        float time = (float)PhotonNetwork.CurrentRoom.CustomProperties["curseTime"];
        // TODO: UPDATE CURSE TIME
    }

    private void SetArtifactDropRate()
    {
        // TODO: UPDATE ARTIFACT DR
    }

    private void ResetLocalPlayer()
    {
        player = PlayerControl.LocalPlayerGameObject;
        Vector3 originalPos = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position;
        player.transform.position = new Vector3(originalPos.x, originalPos.y + 2.0f, originalPos.z);
    }

    private void CurseRandomPlayer()
    {
        int initialCursed = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        PickCursed(PhotonNetwork.PlayerList[initialCursed].ActorNumber);
    }

    public override void OnPlayerLeftRoom(Player playerLeft)
    {
        if (playerLeft.ActorNumber == whichPlayerIsCursed)
        {
            CurseRandomPlayer();
        }
    }
}
