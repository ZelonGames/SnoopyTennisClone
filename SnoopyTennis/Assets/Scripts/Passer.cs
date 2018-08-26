using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passer : MonoBehaviour
{
    #region Instance Fields

    public GameObject ball;
    private Timer timer = null;
    private TimeSkipper timeSkipper = null;

    [Range(0, 20)]
    public int timeSkips = 6;
    [Range(0, 50)]
    public int minSteps = 15;
    [Range(0, 50)]
    public int maxSteps = 30;

    #endregion

    #region Properties

    public int targetLevel { get; private set; }

    #endregion

    #region Events

    void Start()
    {
        timer = GameObjectHelper.GetTimer;
        timeSkipper = new TimeSkipper(timeSkips);
    }

    void Update()
    {
        if (GameManager.GameOver)
            return;

        if (!timer.OnTick)
            return;

        timeSkipper.Update();

        if (timeSkipper.Done)
            CreateBall(transform.position, Random.Range(minSteps, maxSteps));
    }

    #endregion

    #region Methods

    private void CreateBall(Vector2 position, int steps)
    {
        targetLevel = Random.Range(0, GameManager.PasserPaths.Count);
        GameManager.CreateBall(ball, position);
    }

    #endregion
}
