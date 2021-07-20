using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviourPun
{
    #region CharacterParameters
    float moveSpeed = 20f;
    Vector3 forward, right;
    float gravity = 0.0f;
    private bool isCursed = false;
    private bool isInitial = true;
    private int ownerID;
    private Vector3 velocity = Vector3.zero;
    #endregion
    #region ControllerParameters
    CharacterController controller;
    public static PlayerControl LocalPlayerInstance;
    public static GameObject LocalPlayerGameObject;
    #endregion
    #region CameraParameters
    private Vector3 offset;
    private Transform cameraTransform;
    #endregion
    #region LagCompensation
    Vector3 networkPosition;
    Quaternion networkRotation;
    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerControl.LocalPlayerInstance = this;
            PlayerControl.LocalPlayerGameObject = this.gameObject;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        controller = GetComponent<CharacterController>();

        cameraTransform = Camera.main.GetComponent<Transform>();
        offset = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine) { 

            if (Input.anyKey)
            {
                Move();
            }

            ApplyGravity();

            if (cameraTransform != null)
            {
                cameraTransform.position = transform.position + offset;
            }
            else
            {
                cameraTransform = Camera.main.GetComponent<Transform>();
                offset = cameraTransform.position;
            }
        }
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            gravity -= 9.8f;
            controller.Move(new Vector3(0.0f, gravity, 0.0f));
        }
        else
        {
            gravity = 0.0f;
        }
        
    }

    private void Move()
    {
        Vector3 oldPosition = transform.position;
        Vector3 rightMovement = right * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * Input.GetAxis("VerticalKey");

        Vector3 movement = rightMovement + upMovement;

        movement.Normalize();

        controller.Move(movement * moveSpeed * Time.deltaTime);
        velocity = transform.position - oldPosition;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            GameObject collidedPlayer = hit.gameObject;

            if (isCursed)
            {
                GameObject gameManager = GameObject.Find("GameManager");
                int newCursedPlayerNumber = collidedPlayer.GetComponent<PlayerControl>().GetPlayerID();
                gameManager.GetComponent<GameManager>().PickCursed(newCursedPlayerNumber);
            }
        }
        else if (hit.gameObject.tag == "Environment")
        {
            Debug.Log("enviro test");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isCursed);
        }
        else
        {
            this.isCursed = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SetID(int id)
    {
        ownerID = id;
    }

    public int GetPlayerID()
    {
        return ownerID;
    }

    public void SetSpeed()
    {
        moveSpeed = ((float)PhotonNetwork.CurrentRoom.CustomProperties["moveSpeed"] / 10 * moveSpeed);
        Debug.Log(moveSpeed);
    }

    public void BecomeCursed(int playerNumber)
    {
        if (isInitial)
        {
            isInitial = false;
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[playerNumber])
            {
                isCursed = true;
                GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Red");
            }
            else
            {
                isCursed = false;
                GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Blue");
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.CurrentRoom.GetPlayer(playerNumber))
            {
                isCursed = true;
                GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Red");
            }
            else
            {
                isCursed = false;
                GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Blue");
            }
        }
    }
}
