using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Color portalOpenColor;

    private SpriteRenderer _spriteRenderer;
    private Color _defaultInnerColor;
    private Vector2 _portalPosition;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        _portalPosition = transform.position;
        _defaultInnerColor = _spriteRenderer.color;
    }

    public Vector2 GetPosition() { return _portalPosition; }
    public void SetColor(bool isOpen)
    {
        if (isOpen) { _spriteRenderer.color = portalOpenColor; }
        else {  _spriteRenderer.color = _defaultInnerColor;}
    }
}
