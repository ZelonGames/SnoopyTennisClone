using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Text txtScore;

    private GameObject player = null;
    private GameObject passer = null;
    private GameObject enemy = null;

    public static bool gameOver = false;

    #endregion

    #region Properties

    public static int Score { get; private set; }

    #endregion

    #region Events

    private void Start()
    {
        player = GameObject.FindWithTag(GameObjectHelper.Tags.Player);
        enemy = GameObject.FindWithTag(GameObjectHelper.Tags.Enemy);
        passer = GameObject.FindWithTag(GameObjectHelper.Tags.Passer);
    }

    private void Update()
    {
        txtScore.text = "Score: " + Score;

        if (gameOver)
        {
            StopGame();
            ResetGameOnScreenTouch();
        }
    }

    #endregion

    #region Methods

    public static void AddScore(PathStepper pathStepper)
    {
        try
        {
            var scoreController = pathStepper.gameObject.GetComponent<ScoreController>();

            if (pathStepper._MovementDirection == PathStepper.MovementDirection.Right)
            {
                if (scoreController.hasBeenHitByEnemy || !scoreController.hasBeenHitByPlayer)
                    Score += scoreController.Score;

                scoreController.hasBeenHitByPlayer = true;
                scoreController.hasBeenHitByEnemy = false;
            }
            if (pathStepper._MovementDirection == PathStepper.MovementDirection.Left)
            {
                scoreController.hasBeenHitByPlayer = false;
                scoreController.hasBeenHitByEnemy = true;
            }
        }
        catch (System.Exception ex)
        {
        }
    }
    
    private void ResetGameOnScreenTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Ended)
            {
                player.SetActive(true);
                enemy.SetActive(true);
                passer.SetActive(true);
                Score = 0;
                gameOver = false;

                var buttons = GameObject.FindObjectsOfType<Button>();
                foreach(var button in buttons)
                {
                    button.ResetAction();
                }
            }
        }
    }

    private void StopGame()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag(GameObjectHelper.Tags.Ball);

        foreach (var ball in balls)
            Destroy(ball);

        player.GetComponent<PositionLevelGenerator>().ResetPosition();
        player.SetActive(false);
        enemy.GetComponent<AttackStepper>().StepHome();
        enemy.SetActive(false);
        passer.SetActive(false);
    }

    #endregion
}
