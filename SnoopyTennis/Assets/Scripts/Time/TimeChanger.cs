using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class TimeChanger : MonoBehaviour
{
    private Timer timer = null;
    private int startTime = 0;

    void Start()
    {
        timer = gameObject.GetComponent<Timer>();
        startTime = timer.StartTime;
    }

    private void Update()
    {
        if (timer.OnTick())
            timer.SetTime(startTime - (int)Math.Pow(GameManager.Score * 0.5f, 2));
    }
}
