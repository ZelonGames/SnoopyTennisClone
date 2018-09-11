using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private Timer timer = null;

    [SerializeField]
    private GameObject _object;

    [SerializeField]
    private int timeSkips = 3;

    private void Start()
    {
        timer = GameObjectHelper.Timer;
    }

    private void Update()
    {
        if (_object == null || !timer.OnTick(timeSkips))
            return;

        Instantiate(_object);
    }
}
