using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Player : PlayerBase
{
    #region Properties

    public int currentLevel { get; private set; }

    #endregion

    #region Events

    private void Start()
    {
        GenerateLevelPositions();
    }

    #endregion

    #region Methods

    private void GenerateLevelPositions()
    {
        LevelPositions = new List<Vector2>();

        for (int i = 0; i < 3; i++)
        {
            LevelPositions.Add(new Vector2(transform.position.x, transform.position.y + levelDistance * i));
        };
    }

    public void MoveUp()
    {
        if (currentLevel < LevelPositions.Count)
        {
            currentLevel++;
            MoveToCurrentLevelPos();
        }
    }

    public void MoveDown()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            MoveToCurrentLevelPos();
        }
    }

    private void MoveToCurrentLevelPos()
    {
        transform.position = LevelPositions[currentLevel];
    }

    public BallManager GetClosestBall()
    {
        return GameManager.Balls.Where(x => x.CanGetHit(this)).FindMinItem(x => Vector2.Distance(x.transform.position, transform.position));
    }
    #endregion
}
