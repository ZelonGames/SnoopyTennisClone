using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Fields

    private Player player = null;
    private Enemy enemy = null;
    private List<List<Vector2>> currentPaths = null;
    private Timer timer = null;

    public bool movingForward = true;
    private int currentPathIndex = 0;

    private int normalScore = 2;
    private int superScore = 3;

    #endregion

    #region Properties

    private List<Vector2> CurrentPath
    {
        get
        {
            return currentPaths[PathLevel];
        }
    }

    public int Score { get; private set; }
    public int PathLevel { get; private set; }
    public int MovementDirection { get; private set; }

    public bool OnSameLevelAsPlayer
    {
        get
        {
            return PathLevel == player.currentLevel;
        }
    }

    #endregion

    #region Events

    private void Start()
    {
        NormalScore();
        player = GameObject.Find(GameObjectHelper.Names.Player).GetComponent<Player>();
        enemy = GameObject.Find(GameObjectHelper.Names.Enemy).GetComponent<Enemy>();
        var passer = GameObject.Find(GameObjectHelper.Names.Passer).GetComponent<Passer>();
        timer = GameObjectHelper.GetTimer;

        currentPaths = GameManager.PasserPaths;
        PathLevel = passer.targetLevel;
    }

    private void Update()
    {
        if (GameManager.GameOver)
            return;

        if (!timer.OnTick)
            return;

        Vector2 prevPosition = transform.position;

        StepToNextPointInPath();
        ChangeDirectionOnCollision();

        transform.position = CurrentPath[currentPathIndex];

        if ((int)prevPosition.x != (int)transform.position.x)
            MovementDirection = (int)(transform.position.x - prevPosition.x) / (int)Math.Abs(transform.position.x - prevPosition.x);
    }

    #endregion

    #region Methods

    public void NormalScore()
    {
        Score = normalScore;
    }

    public void SuperScore()
    {
        Score = superScore;
    }

    public void StepToNextPointInPath()
    {
        if (movingForward)
        {
            if (currentPathIndex == CurrentPath.Count - 1)
                Land();
            else
                currentPathIndex++;
        }
        else
        {
            if (currentPathIndex == 0)
                Land();
            else
                currentPathIndex--;
        }
    }

    public void ChangeToPlayerPath(int? pathLevel = null)
    {
        currentPaths = GameManager.PlayerPaths;
        if (!pathLevel.HasValue)
            PathLevel = player.currentLevel;
        else
            PathLevel = pathLevel.Value;

        currentPathIndex = GetClosestPointInPath();
    }

    public void ChangeToEnemyPath()
    {
        movingForward = true;

        currentPaths = GameManager.EnemyPaths;
        this.PathLevel = player.currentLevel;

        currentPathIndex = GetClosestPointInPath();
    }

    public void Bounce(BallManager collidedBall)
    {
        if (MovementDirection == 1)
            movingForward = !movingForward;
        else
            ChangeToPlayerPath(collidedBall.PathLevel);

        StepToNextPointInPath();
    }

    private void Land()
    {
        if (LandedOnPlayer(enemy))
            GameManager.DestroyBall(this);
        else if (LandedOnPlayer(player))
            GameManager.GameOver = true;
    }

    private void ChangeDirectionOnCollision()
    {
        foreach (var ball in GameManager.Balls)
        {
            if (ball == this)
                continue;

            if (MovementDirection != ball.MovementDirection && Colliding(ball))
            {
                Bounce(ball);
                ball.Bounce(this);
            }
        }
    }

    private int GetClosestPointInPath()
    {
        Vector2 closestPoint = CurrentPath[0];
        int currentIndex = 0;
        for (int i = 1; i < CurrentPath.Count; i++)
        {
            Vector2 currentPoint = CurrentPath[i];

            if (Vector2.Distance(transform.position, currentPoint) < Vector2.Distance(transform.position, closestPoint))
            {
                closestPoint = currentPoint;
                currentIndex = i;
            }
        }

        return currentIndex;
    }

    public bool LandedOnPlayer(PlayerBase player)
    {
        return player.LevelPositions.Any(l => l.x == transform.position.x && l.y == transform.position.y);
    }

    public bool CanGetHit(PlayerBase player)
    {
        return (player as Enemy != null || OnSameLevelAsPlayer) &&
            Vector2.Distance(transform.position, player.transform.position) <=
            transform.localScale.x * 0.5 + player.transform.localScale.x * 0.5 + player.hitDistance;
    }

    public bool Colliding(BallManager ball)
    {
        return Vector2.Distance(transform.position, ball.transform.position) <= transform.localScale.x * 0.5f + ball.transform.localScale.x * 0.5f;
    }

    #endregion
}
