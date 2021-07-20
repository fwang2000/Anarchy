using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    private int ownerId;
    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Initialize(int playerId, string nickname)
    {
        ownerId = playerId;
        playerName = nickname;
    }

    int GetId()
    {
        return ownerId;
    }
}
