using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy : PlayerBase
{
    #region Fields

    public Player player;
    private Timer timer = null;
    private TimeSkipper stepInTimeSkipper = null;
    private TimeSkipper stepBackTimeSkipper = null;
    private Vector2 position = Vector2.zero;
    private Vector2 homePosition = Vector2.zero;

    [Range(0, 20)]
    public int stepInTimeSkips = 15;
    [Range(0, 20)]
    public int stepBackTimeSkips = 2;

    #endregion

    #region Properties


    public Vector2 AttackPosition { get; private set; }

    private bool Attacking
    {
        get
        {
            return position.x == AttackPosition.x;
        }
    }

    #endregion

    #region Events

    public void Initialize()
    {
        LevelDistance = 1;
        timer = GameObjectHelper.GetTimer;
        stepInTimeSkipper = new TimeSkipper(stepInTimeSkips);
        stepBackTimeSkipper = new TimeSkipper(stepBackTimeSkips);
        position = homePosition = new Vector2(-10, 1.3f);
        AttackPosition = new Vector2(position.x + 3, position.y);
        gameObject.transform.position = homePosition;

        GenerateLevelPositions();
    }

    private void Update()
    {
        if (GameManager.GameOver)
            return;

        if (!timer.OnTick)
            return;

        if (Attacking)
        {
            var balls = GameObject.FindObjectsOfType<BallManager>();

            foreach (var ball in balls)
            {
                if (!ball.CanGetHit(this))
                    continue;

                ball.ChangeToEnemyPath();
                ball.SuperScore();
            }

            stepBackTimeSkipper.Update();
            if (stepBackTimeSkipper.Done)
            {
                Move();
                stepInTimeSkipper.Reset();
            }
        }
        else
        {
            stepInTimeSkipper.Update();

            if (stepInTimeSkipper.Done)
            {
                Move();
                stepBackTimeSkipper.Reset();
            }
        }
    }

    #endregion

    #region Methods

    private void GenerateLevelPositions()
    {
        LevelPositions = new List<Vector2>();

        for (int i = 0; i < 3; i++)
        {
            LevelPositions.Add(new Vector2(AttackPosition.x, AttackPosition.y - LevelDistance + LevelDistance * i));
        };
    }

    private void Move()
    {
        position.x = position.x == homePosition.x ? AttackPosition.x : homePosition.x;
        gameObject.transform.position = position;
    }

    #endregion
}
