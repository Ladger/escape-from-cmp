using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private PointUI pointUI;

    private void Start()
    {
        ActionManager._instance.onBlinkCollect += MovePoint;
    }

    private void OnDestroy()
    {
        ActionManager._instance.onBlinkCollect -= MovePoint;
    }

    public void MovePoint(Vector2 position, int point)
    {
        PointUI pui = Instantiate(pointUI, transform);
        pui.SetPointText(position, point);
    }
}
