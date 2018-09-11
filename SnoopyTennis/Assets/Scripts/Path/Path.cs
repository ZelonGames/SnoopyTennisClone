using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class Path
{
    public List<Vector2> Positions { get; private set; }

    public Vector2 CurrentPosition
    {
        get
        {
            return Positions[CurrentPositionIndex];
        }
    }

    public int CurrentPositionIndex { get; private set; }

    public Path(List<Vector2> positions)
    {
        CurrentPositionIndex = 0;
        Positions = positions;
    }

    public void Jump(Vector2 position)
    {
        CurrentPositionIndex = Positions.IndexOf(position);
    }

    public void NextPosition()
    {
        if (CurrentPositionIndex < Positions.Count - 1)
            CurrentPositionIndex++;
    }

    public void PreviousPosition()
    {
        if (CurrentPositionIndex > 0)
            CurrentPositionIndex--;
    }
}
