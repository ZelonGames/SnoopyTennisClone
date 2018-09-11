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
    private int stepInTimeSkips = 1;

    [SerializeField]
    private int stepOutTimeSkips = 1;

    [SerializeField]
    [Range(0, 10)]
    private float stepDistance = 2;

    #endregion

    #region Properties

    public Vector2 HomePosition { get; private set; }

    private float StepDistance
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

    public bool IsAttacking { get; private set; }

    #endregion

    #region Events

    private void Start()
    {
        HomePosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (!IsAttacking && timer.OnTick(stepInTimeSkips))
            StepIn();
        if (timer.OnTick(stepOutTimeSkips))
            StepHome();
    }

    #endregion

    #region Methods

    private void StepIn()
    {
        Vector2 position = gameObject.transform.position;
        position.x += StepDistance;

        gameObject.transform.position = position;

        IsAttacking = true;
    }

    public void StepHome()
    {
        gameObject.transform.position = HomePosition;

        IsAttacking = false;
    }

    #endregion
}
