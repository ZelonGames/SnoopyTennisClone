using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Instance Fields

    public float timeLimit = 0;
    public float startTime = 1000;
    public bool isCountingDown = true;
    public bool isLooping = true;

    private bool hasTicked = false;

    #endregion

    #region Properties

    public float CurrentTime { get; private set; }

    public bool HasStopped
    {
        get
        {
            return !isLooping && ((isCountingDown && CurrentTime <= timeLimit) || 
                (!isCountingDown && CurrentTime >= timeLimit));
        }
    }

    public bool OnTick
    {
        get
        {
            return hasTicked && CurrentTime == startTime;
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
                hasTicked = true;
                Loop();
            }
        }
        else
        {
            CurrentTime += Time.deltaTime * 1000;
            if (CurrentTime >= timeLimit)
            {
                hasTicked = true;
                Loop();
            }
        }
    }

    private void Loop()
    {
        if (isLooping)
            CurrentTime = startTime;
    }

    #endregion
}
