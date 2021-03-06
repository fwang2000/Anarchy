using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Using code by Photo
public class TopPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";

    [Header("UI References")]
    public Text ConnectionStatusText;

    // Update is called once per frame
    void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }
}
