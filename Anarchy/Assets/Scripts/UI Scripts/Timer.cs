using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Timer : MonoBehaviourPunCallbacks
{
    protected float timeRemaining;
    protected float curseTime;
    protected bool timerIsRunning;
    protected string propertyUpdated;
    protected double startTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timerIsRunning = true;
    }

    protected void RunTimer()
    {
        if (startTime == 0)
        {
            return;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0.0f)
            {
                timeRemaining = curseTime - (float)(PhotonNetwork.Time - startTime);
            }
            else
            {
                timeRemaining = 0.0f;
                timerIsRunning = false;
            }
        }
    }

    public void SetTime(string htkey)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable { { htkey, PhotonNetwork.Time } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
        timerIsRunning = true;
    }
}
