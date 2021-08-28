using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseControl : MonoBehaviour
{
    private CharacterController controller;

    float distance;
    void Start()
    {
        controller = GetComponent<CharacterController>();

        distance = controller.radius + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
