using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStepper : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right
    }

    #region Fields

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Direction direction = Direction.Left;

    [SerializeField]
    private int stayAwayTimeSkips = 1;
    private int stayAwayTimeCounter = 0;

    [SerializeField]
    private int stayAtHomeTimeSkips = 1;
    private int stayAtHomeTimeCounter = 0;

    [SerializeField]
    [Range(0, 10)]
    private float stepDistance = 2;

    #endregion

    #region Properties

    public Vector2 HomePosition { get; private set; }

    public float StepDistance
    {
        get
        {
            switch (direction)
            {
                case Direction.Left:
                    return stepDistance * -1;
                case Direction.Right:
                    return stepDistance;
                default:
                    return stepDistance;
            }
        }
    }

    public bool IsAtHome { get; private set; }

    #endregion

    #region Events

    private void Start()
    {
        ResetStayAwayTimeCounter();
        ResetStayAtHomeTimeCounter();
        HomePosition = gameObject.transform.position;
        IsAtHome = true;
    }

    private void Update()
    {
        if (!timer.OnTick())
            return;

        if (IsAtHome)
        {
            stayAtHomeTimeCounter--;
            ResetStayAwayTimeCounter();
        }
        else
        {
            stayAwayTimeCounter--;
            ResetStayAtHomeTimeCounter();
        }

        if (stayAwayTimeCounter <= 0)
            StepHome(); 
        if (stayAtHomeTimeCounter <= 0)
            StepAway();
    }

    #endregion

    #region Methods

    private void StepAway()
    {
        Vector2 position = gameObject.transform.position;
        position.x += StepDistance;

        gameObject.transform.position = position;

        IsAtHome = false;
    }

    public void StepHome()
    {
        gameObject.transform.position = HomePosition;

        IsAtHome = true;
    }

    private void ResetStayAwayTimeCounter()
    {
        stayAwayTimeCounter = stayAwayTimeSkips;
    }

    private void ResetStayAtHomeTimeCounter()
    {
        stayAtHomeTimeCounter = stayAtHomeTimeSkips;
    }

    #endregion
}
