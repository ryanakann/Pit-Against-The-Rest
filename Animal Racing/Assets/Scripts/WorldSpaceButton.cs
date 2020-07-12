using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class WorldSpaceButton : MonoBehaviour
{
    [SerializeField] private Tween tween;
    [SerializeField] private float transitionTime = 0.25f;
    private bool _mouseOver;
    private bool _mouseClick;
    private float t;
    private Vector3 _initialScale;
    private Camera _camera;

    public UnityEvent OnClick;

    private void Awake()
    {
        t = 0f;
        _initialScale = transform.localScale;

        _camera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.GamePaused) return;
        if (_mouseOver)
        {
            t += 1 / transitionTime * Time.deltaTime;

            if (_mouseClick)
            {
                t = Mathf.Clamp(t, 0f, 2f);
            }
            else
            {
                t = Mathf.Clamp01(t);
            }
        }
        else
        {
            t -= 1 / transitionTime * Time.deltaTime;
            if (t <= 0f)
            {
                t = 0f;
            }
        }

        transform.localScale = _initialScale * Mathf.Lerp(1f, 1.4f, tween.Evaluate(t / 2f));
    }

    private void OnMouseEnter()
    {
        if (GameManager.GamePaused) return;
        SetMouseOver(true);
    }

    private void OnMouseExit()
    {
        if (GameManager.GamePaused) return;
        SetMouseOver(false);
    }

    private void OnMouseDown()
    {
        if (GameManager.GamePaused) return;
        SetMouseClick(true);
    }
    
    private void OnMouseUp()
    {
        if (GameManager.GamePaused) return;
        SetMouseClick(false);
        OnClick?.Invoke();
    }

    private void SetMouseClick(bool value) => _mouseClick = value;
    private void SetMouseOver(bool value) => _mouseOver = value;
}
