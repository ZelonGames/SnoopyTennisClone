using System;
using System.Collections.Generic;
using UnityEngine;

public static class PathHelper
{
    public static List<Vector2> GetCurveBetweenPoints(Vector2 startPos, Vector2 targetPos, Vector2 bezier, float distance)
    {
        var positions = new List<Vector2>();

        int steps = (int)(Vector2.Distance(startPos, targetPos) / distance);

        for (float t = 0; t <= 1; t += 1f / steps)
        {
            var x = (float)(Math.Pow(1 - t, 2) * startPos.x + 2 * (1 - t) * t * bezier.x + Math.Pow(t, 2) * targetPos.x);
            var y = (float)(Math.Pow(1 - t, 2) * startPos.y + 2 * (1 - t) * t * bezier.y + Math.Pow(t, 2) * targetPos.y);

            positions.Add(new Vector2(x, y));
        }

        positions.Add(targetPos);

        return positions;
    }

    public static Vector2 GetBeizer(Vector2 position, Vector2 targetedPosition)
    {
        float x = position.x + (targetedPosition.x - position.x) * 0.5f;
        float y = position.y + targetedPosition.y - position.y + 2;

        return new Vector2(x, y);
    }
}
