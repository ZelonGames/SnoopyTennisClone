using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Fields

    public int timeLimit = 0;
    [SerializeField]
    private int startTime = 1000;
    public bool isCountingDown = true;
    public bool isLooping = true;

    #endregion

    #region Properties

    public int StartTime
    {
        get
        {
            return startTime;
        }
    }

    public float CurrentTime { get; private set; }

    private int Ticks { get; set; }

    private bool HasTicked { get; set; }

    public bool HasStopped
    {
        get
        {
            return !isLooping && ((isCountingDown && CurrentTime <= timeLimit) ||
                (!isCountingDown && CurrentTime >= timeLimit));
        }
    }

    #endregion

    #region Methods

    private void Start()
    {

    }

    private void Update()
    {
        // Stop the timer
        if (!isLooping && ((isCountingDown && CurrentTime <= timeLimit) || (!isCountingDown && CurrentTime >= timeLimit)))
        {
            CurrentTime = timeLimit;
            return;
        }

        if (isCountingDown)
        {
            CurrentTime -= Time.deltaTime * 1000;
            if (CurrentTime <= 0)
            {
                HasTicked = true;
                Loop();
            }
        }
        else
        {
            CurrentTime += Time.deltaTime * 1000;
            if (CurrentTime >= timeLimit)
            {
                HasTicked = true;
                Loop();
            }
        }

        if (HasTicked)
            Ticks++;
    }

    private void Loop()
    {
        if (isLooping)
            CurrentTime = startTime;
    }

    public bool OnTick(int timeskips = 0)
    {
        if (timeskips <= 0)
            return CurrentTime == startTime;

        return CurrentTime == startTime && Ticks % timeskips == 0;
    }

    public void SetTime(int milliseconds)
    {
        startTime = milliseconds;
    }

    #endregion
}
