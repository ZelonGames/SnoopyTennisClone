﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public List<Vector2> LevelPositions { get; protected set; }

    public int levelDistance = 2;
    [Range(0, 10)]
    public int hitDistance = 1;
}