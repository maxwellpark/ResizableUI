using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ResizableElement : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _borderSize = 16;
    [SerializeField] private Vector3 _windowSizeMin = Vector2.zero;
    [SerializeField] private CursorHandler _cursorHandler;

    private bool _isResizing;
    private float _xCoefficient;
    private float _yCoefficient;

    private Vector3 _previousPos;

    private void Update()
    {
        HandleResizing();
    }

    private void HandleResizing()
    {
        UIUtils.GetCursorPosition(_rect, out Vector3 parentPos, out Vector3 localPos, _isResizing);

        if (_isResizing)
        {
            ResizeRect(parentPos);
        }
        // Allow resizing when the cursor is within the rect's borders
        else if (IsWithinBorders(localPos))
        {
            UpdateCoefficients(localPos);

            if (Input.GetMouseButtonDown(0))
                BeginResizing();
        }

        if (Input.GetMouseButtonUp(0))
            StopResizing();

        _previousPos = parentPos;
    }

    public void ResizeRect(Vector3 parentPos)
    {
        Vector2 delta = parentPos - _previousPos;

        delta.x = _xCoefficient * Mathf.Max(
            _windowSizeMin.x - _rect.rect.width, _xCoefficient * delta.x);

        delta.y = _yCoefficient * Mathf.Max(
            _windowSizeMin.y - _rect.rect.height, _yCoefficient * delta.y);

        // Scale x axis
        if (_xCoefficient > 0)
        {
            _rect.sizeDelta += new Vector2(delta.x, 0);
            _rect.anchoredPosition += new Vector2(delta.x * _rect.pivot.x, 0);
        }
        else if (_xCoefficient < 0)
        {
            _rect.sizeDelta -= new Vector2(delta.x, 0);
            _rect.anchoredPosition += new Vector2(delta.x * (1 - _rect.pivot.x), 0);
        }

        // Scale y axis
        if (_yCoefficient > 0)
        {
            _rect.sizeDelta += new Vector2(0, delta.y);
            _rect.anchoredPosition += new Vector2(0, delta.y * _rect.pivot.y);
        }
        else if (_yCoefficient < 0)
        {
            _rect.sizeDelta -= new Vector2(0, delta.y);
            _rect.anchoredPosition += new Vector2(0, delta.y * (1 - _rect.pivot.y));
        }
    }

    // Gets a value to multiply the delta by 
    private float GetResizeCoefficient(float axisPos, float size, float borderSize)
    {
        if (axisPos < borderSize)
            return -1;
        if (axisPos > size - borderSize)
            return 1;
        return 0;
    }

    private void UpdateCoefficients(Vector3 localPos)
    {
        _xCoefficient = GetResizeCoefficient(localPos.x, _rect.rect.width, _borderSize);
        _yCoefficient = GetResizeCoefficient(localPos.y, _rect.rect.height, _borderSize);
    }

    private void BeginResizing()
    {
        CursorHandler.SetCursorType(CursorType.HorizontalResize);
        _isResizing = true;
    }

    private void StopResizing()
    {
        CursorHandler.SetCursorType(CursorType.Pointer);
        _isResizing = false;
    }

    private bool IsWithinRect(Vector3 localPos)
    {
        return localPos.x >= 0
            && localPos.y >= 0
            && localPos.x <= _rect.rect.width
            && localPos.y <= _rect.rect.height;
    }

    private bool IsWithinBorders(Vector3 localPos)
    {
        return localPos.x < _borderSize
            || localPos.y < _borderSize
            || localPos.x > _rect.rect.width - _borderSize
            || localPos.y > _rect.rect.height - _borderSize;
    }
}
