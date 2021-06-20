using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;

public class PlayerControl : MonoBehaviourPun
{
    float moveSpeed = 20f;
    Vector3 forward, right;
    float gravity = 0.0f;

    private bool cursed = true;
    private int ownerID;

    CharacterController controller;

    private Vector3 offset;
    private Transform cameraTransform;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerControl.LocalPlayerInstance = this.gameObject;
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
        if (photonView.IsMine)
        {
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
        Vector3 rightMovement = right * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * Input.GetAxis("VerticalKey");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        Vector3 movement = rightMovement + upMovement;

        movement.Normalize();

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            if (cursed)
            {
                Debug.Log("curse passed on");
                cursed = false;
            }
        }
        else if (hit.gameObject.tag == "Environment")
        {
            Debug.Log("enviro test");
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

    [PunRPC]
    public void SetSpeed()
    {
        moveSpeed = (float)PhotonNetwork.CurrentRoom.CustomProperties["moveSpeed"];
    }
}
