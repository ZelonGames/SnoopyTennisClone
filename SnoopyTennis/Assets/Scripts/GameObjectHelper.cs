using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectHelper
{
    public struct Tags
    {
        public static readonly string Player = "Player";
        public static readonly string Passer = "Passer";
        public static readonly string Enemy = "Enemy";
        public static readonly string Ball = "Ball";
    }

    public static GameManager GameManager
    {
        get
        {
            return GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    public static Timer Timer
    {
        get
        {
            return GameObject.Find("TimeController").GetComponent<Timer>();
        }
    }
}
