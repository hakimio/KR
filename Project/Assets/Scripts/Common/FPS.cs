using UnityEngine;
using System.Collections;

public class FPS: MonoBehaviour
{
    public float updateInterval = 0.5f;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeLeft; // Left time for current interval

    // Use this for initialization
    void Start()
    {
        timeLeft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeLeft <= 0)
        {
            // display two fractional digits (f2 format)
            guiText.text = "" + (accum / frames).ToString("f2");
            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }
}
