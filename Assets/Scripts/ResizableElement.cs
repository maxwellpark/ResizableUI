using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ResizableElement : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _borderSize = 16;
    [SerializeField] private Vector3 _windowSizeMin = Vector2.zero;

    private Vector3 _previousPos;
    private Vector3 _deltaDifference;
    private bool _isResizing;
    private float _xCoefficient;
    private float _yCoefficient;

    public void Resize(Vector3 parentPos)
    {
        Vector2 delta = parentPos - _previousPos - _deltaDifference;
        Vector2 startingDelta = delta;

        delta.x = _xCoefficient * Mathf.Max(
            _windowSizeMin.x - _rect.rect.width, _xCoefficient * delta.x);

        delta.y = _yCoefficient * Mathf.Max(
            _windowSizeMin.y - _rect.rect.height, _yCoefficient * delta.y);

        _deltaDifference = delta - startingDelta;

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

    private float GetResizeCoefficient(float axisPos, float size, float borderSize)
    {
        if (axisPos > -borderSize && axisPos < borderSize)
            return -1;
        if (axisPos < size + borderSize && axisPos > size - borderSize)
            return 1;
        return 0;
    }

    private void UpdateCoefficients(Vector3 localPos)
    {
        _xCoefficient = GetResizeCoefficient(localPos.x, _rect.rect.width, _borderSize);
        _yCoefficient = GetResizeCoefficient(localPos.y, _rect.rect.height, _borderSize);
    }

    private void Update()
    {
        UIUtils.GetCursorPosition(_rect, out Vector3 parentPos, out Vector3 localPos);

        if (!_isResizing)
        {
            UpdateCoefficients(localPos);
        }
        else
        {
            Resize(parentPos);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isResizing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isResizing = false;
            _deltaDifference = Vector2.zero;
        }

        _previousPos = parentPos;
    }
}
