using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PathStepper : MonoBehaviour
{
    public enum MovementDirection
    {
        Left,
        Right,
    }

    public enum PathDirection
    {
        Forward,
        Backward,
    }

    #region Fields

    private Path path;
    [SerializeField]
    private PathGenerator pathGenerator;

    private PathDirection pathDirection = PathDirection.Forward;
    private Timer timer = null;

    [SerializeField]
    private string pathGeneratorObjectName = null;
    [SerializeField]
    private string timerName = null;

    [SerializeField]
    private int timeSkips = 1;

    private float? prevXPos = null;

    [SerializeField]
    private bool destroyOnLastStep = true;

    #endregion

    #region Properties

    public MovementDirection _MovementDirection { get; private set; }

    public int StartLevels
    {
        get
        {
            return pathGenerator.StartLevels;
        }
    }

    public int TargetLevels
    {
        get
        {
            return pathGenerator.TargetLevels;
        }
    }

    public int CurrentStartLevel { get; private set; }
    public int CurrentTargetLevel { get; private set; }

    private int RandomTargetLevel
    {
        get
        {
            return UnityEngine.Random.Range(0, TargetLevels);
        }
    }

    public bool OnLastStep
    {
        get
        {
            return path.CurrentPositionIndex == path.Positions.Count - 1;
        }
    }

    #endregion

    #region Events

    private void Start()
    {
        try
        {
            if (pathGeneratorObjectName != null)
                ChangePathGenerator(pathGeneratorObjectName);

            if (timerName != null)
                timer = GameObject.Find(timerName).GetComponent<Timer>();
        }
        catch (Exception ex)
        {
            throw;
        }

        ChangePath(0, RandomTargetLevel, true);
        StepToCurrentPosition();
    }

    private void Update()
    {
        if (path == null || !timer.OnTick(timeSkips))
            return;

        if (destroyOnLastStep && OnLastStep)
        {
            if (_MovementDirection == MovementDirection.Right)
                GameManager.gameOver = true;
            Destroy(gameObject);
        }

        path.NextPosition();
        switch (pathDirection)
        {
            case PathDirection.Forward:
                StepToNextPosition();
                break;
            case PathDirection.Backward:
                StepToPreviousPosition();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Methods

    public void ChangePath(int startLevel, int targetLevel, bool insansiateNewPath = false)
    {
        Vector2? positionInOldPath = null;

        if (path != null)
            positionInOldPath = new Vector2(path.CurrentPosition.x, path.CurrentPosition.y);

        path = pathGenerator.GetPath(startLevel, targetLevel, insansiateNewPath);

        if (positionInOldPath.HasValue)
            path.Jump(GetClosestPosition(positionInOldPath.Value, path));
        StepToCurrentPosition();
        CurrentStartLevel = startLevel;
        CurrentTargetLevel = targetLevel;
    }

    public void ChangePathGenerator(string pathGeneratorObjectName)
    {
        Vector2? positionInOldPath = null;

        if (path != null)
            positionInOldPath = new Vector2(path.CurrentPosition.x, path.CurrentPosition.y);
        pathGenerator = GameObject.Find(pathGeneratorObjectName).GetComponent<PathGenerator>();

        if (path != null)
        {
            if (positionInOldPath.HasValue)
                path.Jump(GetClosestPosition(positionInOldPath.Value, path));
            StepToCurrentPosition();
        }
    }

    public void ChangeDirection()
    {
        switch (pathDirection)
        {
            case PathDirection.Forward:
                pathDirection = PathDirection.Backward;
                break;
            case PathDirection.Backward:
                pathDirection = PathDirection.Forward;
                break;
            default:
                break;
        }
    }

    public void StepToNextPosition()
    {
        path.NextPosition();
        StepToCurrentPosition();
    }

    public void StepToPreviousPosition()
    {
        path.PreviousPosition();
        StepToCurrentPosition();
    }

    public void StepToCurrentPosition()
    {
        prevXPos = gameObject.transform.position.x;
        gameObject.transform.position = path.CurrentPosition;

        if (prevXPos.HasValue && prevXPos != gameObject.transform.position.x)
        {
            if (gameObject.transform.position.x > prevXPos.Value)
                _MovementDirection = MovementDirection.Right;
            else
                _MovementDirection = MovementDirection.Left;
        }

        
    }

    public Vector2 GetClosestPosition(Vector2 currentPosition, Path path)
    {
        var closestPosition = path.Positions[0];

        for (int i = 1; i < path.Positions.Count; i++)
        {
            var position = path.Positions[i];

            if (Vector2.Distance(currentPosition, position) < Vector2.Distance(currentPosition, closestPosition))
                closestPosition = position;
        }

        return closestPosition;
    }

    #endregion
}
