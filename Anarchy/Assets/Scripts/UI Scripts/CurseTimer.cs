using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CurseTimer : Timer
{
    [SerializeField]
    private UnityEngine.UI.Image timer;

    private Color red = new Color(0.8396226f, 0.06944371f, 0.06072735f);
    private Color green = new Color(0.1305031f, 0.7830189f, 0.2476149f);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        timeRemaining = curseTime = (float)PhotonNetwork.CurrentRoom.CustomProperties["curseTime"];
        propertyUpdated = "StartCurseTime";
    }

    // Update is called once per frame
    void Update()
    {
        base.RunTimer();
        timer.fillAmount = (float) System.Math.Round(timeRemaining / curseTime, 2);
        if (timerIsRunning)
        {
            if (timeRemaining <= 10.0f)
            {
                timer.color = red;
            }
            else
            {
                timer.color = green;
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object propsTime;
        if (propertiesThatChanged.TryGetValue("StartCurseTime", out propsTime))
        {
            startTime = (double)propsTime;
        }
    }
    /*
    public void SetTime()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable { { "StartCurseTime", PhotonNetwork.Time } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
        timer.color = green;
        timerIsRunning = true;
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object propsTime;
        if (propertiesThatChanged.TryGetValue("StartCurseTime", out propsTime))
        {
            startTime = (double)propsTime;
        }
    }*/
}
