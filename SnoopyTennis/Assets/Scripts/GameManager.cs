using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Fields

    public static List<BallManager> Balls { get; private set; }
    public Player player;
    public Passer passer;
    public Enemy enemy;
    public Text txtScore;

    [Range(0, 50)]
    public static int pathSteps = 20;
    public static int score = 0;

    private static bool gameOver = false;

    #endregion

    #region Properties

    public static List<List<Vector2>> PlayerPaths { get; private set; }
    public static List<List<Vector2>> PasserPaths { get; private set; }
    public static List<List<Vector2>> EnemyPaths { get; private set; }

    public static bool GameOver
    {
        get { return gameOver; }
        set
        {
            gameOver = value;
        }
    }

    #endregion

    #region Events

    private void Start()
    {
        player.Initialize();
        enemy.Initialize();

        Balls = new List<BallManager>();
        GeneratePlayerPaths();
        GeneratePasserPaths();
        GenerateEnemyPaths();

        //AddTextToCanvas("Hejsan", 27, new Vector2(100, 0));
    }

    private void Update()
    {
        txtScore.text = "Score: " + score;
        /*if (GameOver)
        {
            if (txtScore.gameObject.activeSelf)
                txtScore.gameObject.SetActive(false);
        }*/
    }

    private void OnMouseDown()
    {
        if (GameOver)
            RestartGame();
    }

    #endregion

    #region Methods

    public static Text AddTextToCanvas(string textString, int fontSize, Vector2 position)
    {
        GameObject canvas = GameObject.Find("Canvas");
        Text text = canvas.AddComponent<Text>();
        text.text = textString;
        text.fontSize = fontSize;
        text.transform.position = position;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;

        return text;
    }

    public static void CreateBall(GameObject ball, Vector2 position)
    {
        var newball = Instantiate(ball);
        newball.transform.position = position;
        Balls.Add(newball.GetComponent<BallManager>());
    }

    public static void DestroyBall(BallManager ball)
    {
        Destroy(ball.gameObject);
        Balls.Remove(ball);
    }

    private void RestartGame()
    {
        GameOver = false;
        foreach(var ball in Balls)
        {
            DestroyBall(ball);
        }
    }

    private void GeneratePlayerPaths()
    {
        PlayerPaths = new List<List<Vector2>>();

        for (int i = 0; i < player.LevelPositions.Count; i++)
        {
            Vector2 currentPlayerLevelPosition = player.LevelPositions[i];
            Vector2 currentEnemyLevelPosition = enemy.LevelPositions[i];

            Vector2 beizer = PathHelper.GetBeizer(currentPlayerLevelPosition, currentEnemyLevelPosition);
            PlayerPaths.Add(PathHelper.GetCurveBetweenPoints(currentPlayerLevelPosition, currentEnemyLevelPosition, beizer, pathSteps));
        }
    }

    private void GeneratePasserPaths()
    {
        PasserPaths = new List<List<Vector2>>();

        for (int i = 0; i < player.LevelPositions.Count; i++)
        {
            Vector2 currentPlayerLevelPosition = player.LevelPositions[i];

            Vector2 beizer = PathHelper.GetBeizer(passer.transform.position, currentPlayerLevelPosition);
            PasserPaths.Add(PathHelper.GetCurveBetweenPoints(passer.transform.position, currentPlayerLevelPosition, beizer, pathSteps));
        }
    }

    private void GenerateEnemyPaths()
    {
        EnemyPaths = new List<List<Vector2>>();

        for (int i = 0; i < player.LevelPositions.Count; i++)
        {
            Vector2 currentPlayerLevelPosition = player.LevelPositions[i];

            Vector2 beizer = PathHelper.GetBeizer(enemy.AttackPosition, currentPlayerLevelPosition);
            EnemyPaths.Add(PathHelper.GetCurveBetweenPoints(enemy.AttackPosition, currentPlayerLevelPosition, beizer, pathSteps));
        }
    }

    #endregion
}
