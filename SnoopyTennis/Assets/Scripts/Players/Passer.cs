using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Passer : PlayerBase
{
    #region Fields

    public GameObject ball;
    private Timer timer = null;
    private TimeSkipper timeSkipper = null;

    [Range(0, 20)]
    public int timeSkips = 6;

    #endregion

    #region Properties

    public int targetLevel { get; private set; }

    #endregion

    #region Events

    private void Start()
    {
        timer = GameObjectHelper.GetTimer;
        timeSkipper = new TimeSkipper(timeSkips);
    }

    private void Update()
    {
        if (GameManager.GameOver)
            return;

        if (!timer.OnTick)
            return;

        timeSkipper.Update();

        if (timeSkipper.Done)
            CreateBall(transform.position);
    }

    #endregion

    #region Methods

    private void CreateBall(Vector2 position)
    {
        targetLevel = Random.Range(0, GameManager.PasserPaths.Count);
        GameManager.CreateBall(ball, position);
    }

    #endregion
}
