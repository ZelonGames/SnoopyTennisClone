using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectHelper
{
    public struct Names
    {
        public static readonly string Player = "Player";
        public static readonly string Passer = "Passer";
        public static readonly string Enemy = "Enemy";
    }

    public static Timer GetTimer
    {
        get
        {
            return GameObject.Find("TimeController").GetComponent<Timer>();
        }
    }
}
