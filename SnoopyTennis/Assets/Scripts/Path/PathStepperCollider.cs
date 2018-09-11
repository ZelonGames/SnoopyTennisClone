using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathStepper))]
public class PathStepperCollider : MonoBehaviour
{
    #region Fields

    private Timer timer = null;
    
    [SerializeField]
    private string timerName = null;

    #endregion

    #region Properties

    public PathStepper PathStepper { get; private set; }

    public float Radius
    {
        get
        {
            return gameObject.transform.localScale.x * 0.5f;
        }
    }

    #endregion

    #region Events

    private void Start()
    {
        PathStepper = gameObject.GetComponent<PathStepper>();
        if (timerName != null)
            timer = GameObject.Find(timerName).GetComponent<Timer>();
    }

    private void Update()
    {
        if (!timer.OnTick(1))
            return;

        var pathStepperColliders = GameObject.FindObjectsOfType<PathStepperCollider>();
        foreach (var pathStepperCollider in pathStepperColliders)
        {
            if (this == pathStepperCollider)
                continue;

            if (PathStepper._MovementDirection != pathStepperCollider.PathStepper._MovementDirection && Collides(pathStepperCollider))
            {
                PathStepper.ChangeDirection();
                pathStepperCollider.PathStepper.ChangeDirection();
            }
        }
    }

    #endregion

    #region Methods

    private bool Collides(PathStepperCollider pathStepperCollider)
    {
        return Vector2.Distance(gameObject.transform.position, pathStepperCollider.gameObject.transform.position) <= Radius + pathStepperCollider.Radius;
    }

    #endregion
}
