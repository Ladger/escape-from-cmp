using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Vector2 pointOffset;

    public void SetPointText(Vector2 position, int point)
    {
        pointText.text = "+" + point.ToString();
        gameObject.transform.position = position;

        Sequence seq = DOTween.Sequence();

        seq.Join(transform.DOMove(position + pointOffset, 1f));
        seq.Join(pointText.DOFade(0, 1f));

        seq.Play().OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
