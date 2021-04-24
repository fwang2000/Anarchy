using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviourPun
{
    float moveSpeed = 20f;
    Vector3 forward, right;
    float gravity = 0.0f;

    private bool cursed = true;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        controller = GetComponent<CharacterController>();
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

        // transform.forward = heading;

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
}
