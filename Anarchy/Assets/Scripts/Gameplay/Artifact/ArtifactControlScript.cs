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

    private Color beaconColor = new Color(0, 252, 255);

    private bool artifactActivated = false;
    private bool timerIsRunning = true;
    private float timeRemaining = 10.0f;

    private void Start()
    {
        beacon.GetComponent<Renderer>().material.color = beaconColor;
        ArtifactButton = ArtifactButtonObject.GetComponent<Button>();
        PlayerCapsule = PlayerControl.LocalPlayerGameObject;
    }

    private void Update()
    {
        if (artifactActivated)
        {
            StartArtifactTask();
        }
        else
        {
            CheckPlayerDistance();
        }
    }

    private void CheckPlayerDistance()
    {
        if (PlayerCapsule.transform)
        {
            float distance = Vector3.Distance(PlayerCapsule.transform.position, transform.position);
            
            if (distance < 5.0f)
            {
                ArtifactButtonObject.SetActive(true);
            }
            else
            {
                ArtifactButtonObject.SetActive(false);
                ArtifactPanel.SetActive(false);
                ArtifactTracer.SetActive(false);
            }
        }
    }

    public void OnInteractButtonActivated()
    {
        SetArtifactTask(true);
        GetComponent<PhotonView>().RPC("DeactivateArtifact", RpcTarget.AllBuffered);
    }

    private void SetArtifactTask(bool isActive)
    {
        artifactActivated = isActive;
        ArtifactPanel.SetActive(isActive);
        ArtifactTracer.SetActive(isActive);
        Globals.freezePlayer = isActive;
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
                timeRemaining = 0.0f;
                timerIsRunning = false;
                ArtifactTracer.GetComponent<ArtifactTracer>().DestroyDrawPrefabs();
                SetArtifactTask(false);

            }
        }
    }

    [PunRPC]
    void DeactivateArtifact()
    {
        ArtifactButton.GetComponent<Button>().interactable = false;
        ChangeArtifact();
    }

    void ChangeArtifact()
    {
        beacon.GetComponent<Renderer>().material.color = new Color(1, 0, 0.108f);
    }
}
