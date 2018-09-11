using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [HideInInspector]
    public bool hasBeenHitByPlayer = false;
    [HideInInspector]
    public bool hasBeenHitByEnemy = false;

    [SerializeField]
    private int scoreValue = 1;

    [SerializeField]
    private int speicalScoreValue = 2;

    public int Score
    {
        get
        {
            return hasBeenHitByEnemy ? speicalScoreValue : scoreValue;
        }
    }
}
