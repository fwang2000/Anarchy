using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoundTimer : Timer
{
    [SerializeField]
    private UnityEngine.UI.Text timerText;

    private Color red = new Color(0.8396226f, 0.06944371f, 0.06072735f);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        timeRemaining = (float)PhotonNetwork.CurrentRoom.CustomProperties["roundTime"];
        InvokeRepeating("UpdateTimerText", 1.0f, 1.0f);
    }

    void UpdateTimerText()
    {
        int minutes = int.Parse(timerText.text.Substring(0, 2));
        int seconds = int.Parse(timerText.text.Substring(3));

        if (minutes == 0 && seconds == 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("MainGame");
            }
            return;
        }

        seconds--;

        if (seconds < 0)
        {
            seconds = 59;
            minutes--;
        }

        string minutesText = "0" + minutes.ToString();
        string secondsText;

        if (seconds < 10)
        {
            secondsText = "0" + seconds.ToString();
        }
        else
        {
            secondsText = seconds.ToString();
        }

        timerText.text = minutesText + ":" + secondsText;

        if (minutes == 0 && seconds <= 30)
        {
            timerText.color = red;
        }
        else
        {
            timerText.color = Color.black;
        }
    }
}
