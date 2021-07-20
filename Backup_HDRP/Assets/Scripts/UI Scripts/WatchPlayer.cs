using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchPlayer : MonoBehaviour
{
    // THIS SCRIPT IS FOR BETA TESTING --> NOT USING IN FINAL PRODUCT

    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
