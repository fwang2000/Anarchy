using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;

public class GameSettingsController : MonoBehaviour
{
    [SerializeField]
    private GameObject moveSpeed;

    [SerializeField]
    private GameObject roundTimer;

    [SerializeField]
    private GameObject curseTimer;

    [SerializeField]
    private GameObject artifactDropRate;

    private enum DropRate { LOW, MEDIUM, HIGH, OFF }

    private void Start()
    {
        TextMeshProUGUI artifactDRText = artifactDropRate.transform.Find("ArtifactDRText/Text").GetComponent<TextMeshProUGUI>();
        artifactDRText.text = DropRate.LOW.ToString();
    }

    #region ButtonFunctions

    public void OnMoveSpeedAddClicked()
    {
        TextMeshProUGUI moveSpeedText = moveSpeed.transform.Find("SpeedText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(moveSpeedText.text, out int number);
        if (isInt)
        {
            if (number < 15)
            {
                number++;
                moveSpeedText.text = number.ToString();
                updateRoomProps("moveSpeed", number);
            }
        }
    }

    public void OnMoveSpeedMinusClicked()
    {
        TextMeshProUGUI moveSpeedText = moveSpeed.transform.Find("SpeedText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(moveSpeedText.text, out int number);
        if (isInt)
        {
            if (number > 1)
            {
                number--;
                moveSpeedText.text = number.ToString();
                updateRoomProps("moveSpeed", number);
            }
        }
    }

    public void OnRoundTimerAddClicked()
    {
        TextMeshProUGUI roundTimerText = roundTimer.transform.Find("RoundTimeText/Text").GetComponent<TextMeshProUGUI>();
        bool isDouble = double.TryParse(roundTimerText.text, out double number);
        if (isDouble)
        {
            if (number < 10.0)
            {
                number += 0.5;
                roundTimerText.text = number.ToString();
                updateRoomProps("roundTime", (float)number);
            }
        }
    }

    public void OnRoundTimerMinusClicked()
    {
        TextMeshProUGUI roundTimerText = roundTimer.transform.Find("RoundTimeText/Text").GetComponent<TextMeshProUGUI>();
        bool isDouble = double.TryParse(roundTimerText.text, out double number);

        TextMeshProUGUI curseTimerText = curseTimer.transform.Find("CurseTimeText/Text").GetComponent<TextMeshProUGUI>();
        double.TryParse(curseTimerText.text, out double curseTime);

        if (isDouble)
        {
            if (number > 1.00)
            {
                number -= 0.5;

                if (curseTime > number)
                {
                    curseTimerText.text = number.ToString();
                }

                roundTimerText.text = number.ToString();
                updateRoomProps("roundTime", (float) number);
            }
        }
    }

    public void OnCurseTimerAddClicked()
    {
        TextMeshProUGUI curseTimerText = curseTimer.transform.Find("CurseTimeText/Text").GetComponent<TextMeshProUGUI>();
        bool isDouble = double.TryParse(curseTimerText.text, out double number);

        TextMeshProUGUI roundTimerText = roundTimer.transform.Find("RoundTimeText/Text").GetComponent<TextMeshProUGUI>();
        double.TryParse(roundTimerText.text, out double roundTime);

        if (isDouble)
        {
            if (number < 10.0 && number < roundTime)
            {
                number += 0.5;
                curseTimerText.text = number.ToString();
                updateRoomProps("curseTime", (float) number);
            }
        }
    }

    public void OnCurseTimerMinusClicked()
    {
        TextMeshProUGUI curseTimerText = curseTimer.transform.Find("CurseTimeText/Text").GetComponent<TextMeshProUGUI>();
        bool isDouble = double.TryParse(curseTimerText.text, out double number);
        if (isDouble)
        {
            if (number > 1)
            {
                number -= 0.5;
                curseTimerText.text = number.ToString();
                updateRoomProps("curseTime", (float) number);
            }
        }
    }

    public void OnArtifactDRAddClicked()
    {
        TextMeshProUGUI artifactDRText = artifactDropRate.transform.Find("ArtifactDRText/Text").GetComponent<TextMeshProUGUI>();
        string text = artifactDRText.text;
        switch(text)
        {
            case "LOW": 
                artifactDRText.text = DropRate.MEDIUM.ToString();
                break;
            case "MEDIUM":
                artifactDRText.text = DropRate.HIGH.ToString();
                break;
            case "HIGH":
                artifactDRText.text = DropRate.OFF.ToString();
                break;
            case "OFF":
                artifactDRText.text = DropRate.LOW.ToString();
                break;
        }
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        props["artifactsDR"] = artifactDRText.text;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public void OnArtifactDRMinusClicked()
    {
        TextMeshProUGUI artifactDRText = artifactDropRate.transform.Find("ArtifactDRText/Text").GetComponent<TextMeshProUGUI>();
        string text = artifactDRText.text;
        switch (text)
        {
            case "LOW":
                artifactDRText.text = DropRate.OFF.ToString();
                break;
            case "MEDIUM":
                artifactDRText.text = DropRate.LOW.ToString();
                break;
            case "HIGH":
                artifactDRText.text = DropRate.MEDIUM.ToString();
                break;
            case "OFF":
                artifactDRText.text = DropRate.HIGH.ToString();
                break;
        }
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        props["artifactsDR"] = artifactDRText.text;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    private void updateRoomProps(string propName, float value)
    {
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        props[propName] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    #endregion
}
