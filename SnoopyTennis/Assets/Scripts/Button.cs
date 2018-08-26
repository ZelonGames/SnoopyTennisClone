﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public enum ButtonType
    {
        Up,
        Down,
        Hit,
    }

    #region Fields

    public ButtonType buttonType;
    public GameObject player;
    private Player playerComponent = null;
    private Timer timer = null;

    private bool readToMakeAction = false;

    #endregion

    #region Events

    private void Start()
    {
        timer = GameObjectHelper.GetTimer;
        playerComponent = player.GetComponent<Player>();
    }


    private void Update()
    {
        if (GameManager.GameOver)
            return;

        if (!timer.OnTick)
            return;

        if (readToMakeAction)
        {
            switch (buttonType)
            {
                case ButtonType.Up:
                    playerComponent.MoveUp();
                    break;
                case ButtonType.Down:
                    playerComponent.MoveDown();
                    break;
                case ButtonType.Hit:
                    BallManager closestBall = playerComponent.GetClosestBall();
                    if (closestBall == null)
                        break;

                    GameManager.score += closestBall.Score;
                    closestBall.movingForward = true;
                    closestBall.ChangeToPlayerPath();
                    closestBall.NormalScore();
                    break;
                default:
                    break;
            }

            readToMakeAction = false;
        }
    }

    private void OnMouseDown()
    {
        readToMakeAction = true;
    }

    #endregion
}
