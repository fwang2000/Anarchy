using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ArtifactControlScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ArtifactButtonObject;
    private Button ArtifactButton;

    [SerializeField]
    private GameObject ArtifactPanel;

    [SerializeField]
    private GameObject ArtifactTracer;

    [SerializeField]
    private GameObject beacon;

    private GameObject PlayerCapsule;

    private Color openBeaconColor = new Color(0, 252, 255);
    private Color activeBeaconColor = new Color(1, 0, 0.108f);

    private bool artifactUsed = false;
    private bool timerIsRunning = true;
    private float timeRemaining = 10.0f;

    private void Start()
    {
        beacon.GetComponent<Renderer>().material.color = openBeaconColor;
        ArtifactButton = ArtifactButtonObject.GetComponent<Button>();
        PlayerCapsule = PlayerControl.LocalPlayerGameObject;
    }

    private void Update()
    {
        if (artifactUsed)
        {
            StartArtifactTask();
        }

        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        if (PlayerCapsule.transform)
        {
            float distance = Vector3.Distance(PlayerCapsule.transform.position, transform.position);
            
            if (distance <= 5.0f && !artifactUsed)
            {
                ArtifactButtonObject.SetActive(true);
            }
            else if (distance > 5.0f)
            {
                ArtifactButtonObject.SetActive(false);
                if (artifactUsed)
                {
                    SetArtifactUsed(false);
                    SetArtifactTask(false);
                    SetArtifact(10.0f, true);
                }
                else
                {
                    ArtifactPanel.SetActive(false);
                    ArtifactTracer.SetActive(false);
                }
            }
        }
    }

    public void OnInteractButtonActivated()
    {
        SetArtifactTask(true);
    }

    private void SetArtifactUsed(bool isActive)
    {
        artifactUsed = isActive;
    }

    private void SetArtifactTask(bool isActive)
    {
        ArtifactPanel.SetActive(isActive);
        ArtifactTracer.SetActive(isActive);
        Globals.freezePlayer = isActive;
        GetComponent<PhotonView>().RPC("ActivateArtifact", RpcTarget.AllBuffered, !isActive);
    }

    private void StartArtifactTask()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0.0f)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                SetArtifactUsed(true);
                SetArtifact(0.0f, false);
                ArtifactTracer.GetComponent<ArtifactTracer>().DestroyDrawPrefabs();
                SetArtifactTask(false);
            }
        }
    }

    [PunRPC]
    void ActivateArtifact(bool isButtonActive)
    {
        ArtifactButton.GetComponent<Button>().interactable = isButtonActive;
        ChangeArtifact(isButtonActive);
    }

    void ChangeArtifact(bool isButtonActive)
    {
        beacon.GetComponent<Renderer>().material.color = isButtonActive ? openBeaconColor : activeBeaconColor;
    }

    private void SetArtifact(float time, bool isRunning)
    {
        timeRemaining = time;
        timerIsRunning = isRunning;
        ArtifactTracer.GetComponent<ArtifactTracer>().DestroyDrawPrefabs();
    }
}
