using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    void Start()
    {
        LevelManager._instance.BuildMap(0);
    }
}
