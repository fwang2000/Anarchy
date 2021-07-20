using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointController : MonoBehaviour
{
    public static SpawnpointController singletonInstance;

    public Transform[] spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        singletonInstance = this;
    }
}
