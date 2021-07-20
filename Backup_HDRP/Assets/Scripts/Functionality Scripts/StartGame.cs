using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartGame : MonoBehaviourPunCallbacks
{
    public void OnStartButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MainGame");
        }
    }
}
