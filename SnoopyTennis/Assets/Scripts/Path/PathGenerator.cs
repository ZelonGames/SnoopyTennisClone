using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionLevelGenerator))]
public sealed class PathGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private PositionLevelGenerator targetPositionLevels;

    private List<List<Path>> paths = new List<List<Path>>();
    private PositionLevelGenerator startPositionLevels;

    [SerializeField]
    private float distance = 0.5f;

    #endregion

    #region Properties

    public int StartLevels
    {
        get
        {
            return startPositionLevels.Levels;
        }
    }

    public int TargetLevels
    {
        get
        {
            return targetPositionLevels.Levels;
        }
    }

    #endregion

    #region Events

    private void Start()
    {
        startPositionLevels = gameObject.GetComponent<PositionLevelGenerator>();

        for (int s = 0; s < startPositionLevels.LevelPositions.Count; s++)
        {
            Vector2 currentStartPosition = startPositionLevels.LevelPositions[s];
            paths.Add(new List<Path>());

            for (int t = 0; t < targetPositionLevels.LevelPositions.Count; t++)
            {
                Vector2 currentTargetPosition = targetPositionLevels.LevelPositions[t];
                Vector2 currentBeizer = PathHelper.GetBeizer(currentStartPosition, currentTargetPosition);

                List<Vector2> currentPath = PathHelper.GetCurveBetweenPoints(currentStartPosition, currentTargetPosition, currentBeizer, distance);
                paths[s].Add(new Path(currentPath));
            }
        }
    }

    #endregion

    #region Methods

    public Path GetPath(int level, bool instantiateNewPath = false)
    {
        Path path = paths[level][level];
        return instantiateNewPath ? new Path(path.Positions) : path;
    }

    public Path GetPath(int startLevel, int targetLevel, bool instantiateNewPath = false)
    {
        if (targetLevel > paths[startLevel].Count - 1)
            targetLevel = paths[startLevel].Count - 1;

        Path path = paths[startLevel][targetLevel];
        return instantiateNewPath ? new Path(path.Positions) : path;
    }

    #endregion
}
