using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathGenerator))]
public class PathSwitcher : MonoBehaviour
{
    private enum SwitchType
    {
        SameLevel,
        AnyLevel,
    }

    #region Fields

    [SerializeField]
    private SwitchType switchType = SwitchType.SameLevel;

    [SerializeField]
    private PositionLevelGenerator targetLevelManager;

    [SerializeField]
    private Button button;

    private PositionLevelGenerator positionLevelManager = null;

    [SerializeField]
    private float hitDistance = 2;

    #endregion

    #region Events

    private void OnEnable()
    {
        if (button != null)
            button.OnButtonPressed += SwitchPath;
    }

    private void OnDisable()
    {
        if (button != null)
            button.OnButtonPressed -= SwitchPath;
    }

    void Start()
    {
        if (switchType == SwitchType.SameLevel)
            positionLevelManager = gameObject.GetComponent<PositionLevelGenerator>();
    }

    private void Update()
    {
        if (button == null)
            SwitchPath();
    }

    #endregion

    #region Methods

    public void SwitchPath()
    {
        var pathSteppers = FindObjectsOfType<PathStepper>();

        foreach (var pathStepper in pathSteppers)
        {
            if (CanGetHit(pathStepper))
            {
                int targetLevel = pathStepper.CurrentTargetLevel;
                pathStepper.ChangePathGenerator(gameObject.name);
                pathStepper.ChangePath(targetLevel, GetTargetLevel(pathStepper), true);
                if (button != null)
                    GameManager.IncreaseScore();
            }
        }
    }

    private int GetTargetLevel(PathStepper pathStepper)
    {
        return targetLevelManager != null ? targetLevelManager.CurrentLevel : pathStepper.CurrentTargetLevel;
    }

    private bool CanGetHit(PathStepper pathStepper)
    {
        var closeEnough = Mathf.Abs(pathStepper.gameObject.transform.position.x - gameObject.transform.position.x) <= hitDistance;
        switch (switchType)
        {
            case SwitchType.SameLevel:
                return closeEnough && pathStepper.CurrentTargetLevel == positionLevelManager.CurrentLevel;
            case SwitchType.AnyLevel:
                return closeEnough;
            default:
                return false;
        }

    }

    #endregion
}
