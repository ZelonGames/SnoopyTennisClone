using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public sealed class Button : MonoBehaviour
{
    #region Fields

    public delegate void ButtonPressed();
    public event ButtonPressed OnButtonPressed;

    private BoxCollider2D collider = null;

    private SpriteRenderer spriteRenderer = null;

    private Timer timer = null;

    [SerializeField]
    private Color pressedColor;
    private Color standByColor;

    private Touch touchBegan;

    private bool readyToMakeAction;

    [SerializeField]
    private int timeSkips = 1;

    #endregion

    #region Events

    private void Start()
    {
        Input.multiTouchEnabled = true;
        timer = GameObjectHelper.Timer;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        standByColor = spriteRenderer.color;

        collider = gameObject.GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (GameManager.gameOver)
            return;

        DetectTouch();

        if (!timer.OnTick(timeSkips))
            return;

        if (readyToMakeAction)
        {
            if (OnButtonPressed != null)
                OnButtonPressed();

            readyToMakeAction = false;
        }
    }

    private void OnMouseUp()
    {
        readyToMakeAction = true;
    }

    #endregion

    #region Methods

    private void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                if (collider.OverlapPoint(worldPoint))
                {
                    if (touch.phase == TouchPhase.Began)
                        ChangeColor(pressedColor);
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        ChangeColor(standByColor);
                        readyToMakeAction = true;
                    }
                }
            }
        }
    }

    private void ChangeColor(Color color)
    {
        spriteRenderer.color = new Color(color.r, color.g, color.b);
    }

    public void ResetAction()
    {
        if (!readyToMakeAction)
            return;

        readyToMakeAction = false;
        ChangeColor(standByColor);
    }

    #endregion
}
