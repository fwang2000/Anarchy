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
    private GameObject beacon;

    private GameObject PlayerCapsule;

    private Color beaconColor = new Color(0, 252, 255);

    private void Start()
    {
        beacon.GetComponent<Renderer>().material.color = beaconColor;
        ArtifactButton = ArtifactButtonObject.GetComponent<Button>();
        PlayerCapsule = PlayerControl.LocalPlayerGameObject;
    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    void CheckPlayerDistance()
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
            }
        }
    }

    public void OnInteractButtonActivated()
    {
        Debug.Log("Deactivate");
        GetComponent<PhotonView>().RPC("DeactivateArtifact", RpcTarget.AllBuffered);
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
