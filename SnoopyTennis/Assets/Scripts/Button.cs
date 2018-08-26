using System.Collections;
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
    private Player player = null;
    private Timer timer = null;

    private bool readToMakeAction = false;

    #endregion

    #region Events

    private void Start()
    {
        timer = GameObjectHelper.GetTimer;
        player = GameObject.Find(GameObjectHelper.Names.Player).GetComponent<Player>();
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
                    player.MoveUp();
                    break;
                case ButtonType.Down:
                    player.MoveDown();
                    break;
                case ButtonType.Hit:
                    BallManager closestBall = player.GetClosestBall();
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
