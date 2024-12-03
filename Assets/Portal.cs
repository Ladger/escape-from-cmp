using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Color portalOpenColor;

    private SpriteRenderer _spriteRenderer;
    private Color _defaultInnerColor;
    private Vector2 _portalPosition;
    private bool _isOpen = false;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        _portalPosition = transform.position;
        _defaultInnerColor = _spriteRenderer.color;

        SetPortalState(_isOpen);
    }

    public void SetPortalState(bool isOpen) { 
        _isOpen = isOpen;
        
        SetColor(_isOpen);
    }
    public bool IsPortalOpen() { return _isOpen;}
    public Vector2 GetPosition() { return _portalPosition; }


    private void SetColor(bool isOpen)
    {
        if (isOpen) { _spriteRenderer.color = portalOpenColor; }
        else {  _spriteRenderer.color = _defaultInnerColor;}
    }
}
