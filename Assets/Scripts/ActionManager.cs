using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
    public Action onMazeFinish;
    public Action onMazeChange;

    public Action onTeleport;
}
