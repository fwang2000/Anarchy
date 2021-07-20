using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeRemaining;
    private bool timerIsRunning;

    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 10.0f;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = timeRemaining.ToString("F2");
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
            }

        }
    }

    public void SetTime(float time)
    {
        timeRemaining = time;
    }
}
