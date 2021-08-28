using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class PlayerNameTag : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    private TextMeshProUGUI nickname;

    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }

    private void SetName()
    {
        nickname.text = photonView.Owner.NickName; 
    }
}
