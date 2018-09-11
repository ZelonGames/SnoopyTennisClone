using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public sealed class PositionLevelGenerator : MonoBehaviour
{
    #region Fields

    public Button buttonMoveUp;
    public Button buttonMoveDown;

    [SerializeField]
    private int levels = 3;
    [SerializeField]
    private int startLevel = 0;
    [SerializeField]
    private int levelDistance = 2;

    #endregion

    #region Properties

    public List<Vector2> LevelPositions { get; private set; }

    public int LevelDistance
    {
        get
        {
            return levelDistance;
        }
    }
    public int CurrentLevel { get; private set; }

    public int Levels
    {
        get
        {
            return levels;
        }
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EnableButtons();
    }

    private void OnDisable()
    {
        DisableButtons();
    }

    private void Start()
    {
        LevelPositions = new List<Vector2>();

        for (int i = 0; i < levels; i++)
        {
            LevelPositions.Add(new Vector2(transform.position.x, transform.position.y + LevelDistance * i));
        };

        CurrentLevel = startLevel;
        MoveToCurrentLevelPos();
    }

    #endregion

    #region Methods

    public void MoveToCurrentLevelPos()
    {
        transform.position = LevelPositions[CurrentLevel];
    }

    private void MoveUp()
    {
        if (CurrentLevel < LevelPositions.Count - 1)
        {
            CurrentLevel++;
            MoveToCurrentLevelPos();
        }
    }

    private void MoveDown()
    {
        if (CurrentLevel > 0)
        {
            CurrentLevel--;
            MoveToCurrentLevelPos();
        }
    }

    private void EnableButtons()
    {
        if (buttonMoveUp != null)
            buttonMoveUp.OnButtonPressed += MoveUp;
        if (buttonMoveDown != null)
            buttonMoveDown.OnButtonPressed += MoveDown;
    }

    private void DisableButtons()
    {
        if (buttonMoveUp != null)
            buttonMoveUp.OnButtonPressed -= MoveUp;
        if (buttonMoveDown != null)
            buttonMoveDown.OnButtonPressed -= MoveDown;
    }

    public void ResetPosition()
    {
        CurrentLevel = startLevel;
        MoveToCurrentLevelPos();
    }

    #endregion
}
