using UnityEngine;

public enum CursorType
{
    Pointer, VerticalResize, HorizontalResize,
    TopLeftResize, TopRightResize,
    BottomLeftResize, BottomRightResize
}

public class CursorHandler : MonoBehaviour
{
    private CursorType _currentCursorType;

    [SerializeField] private Texture2D _pointerCursor = null;
    [SerializeField] private Texture2D _horizontalCursor;
    [SerializeField] private Texture2D _verticalCursor;

    private void Start()
    {
        //ResizableElement.onBorderEnter += SetCursor;
    }

    public void SetCursorType(CursorType cursorType)
    {
        _currentCursorType = cursorType;
    }

    public void ResetCursor()
    {
        _currentCursorType = CursorType.Pointer;
        SetCursor(_currentCursorType);
    }

    public void SetCursor(CursorType cursorType)
    {
        _currentCursorType = cursorType;
        Texture2D cursorTexture = GetCursorTexture();
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private Texture2D GetCursorTexture()
    {
        return _currentCursorType switch
        {
            CursorType.VerticalResize => _verticalCursor,
            CursorType.HorizontalResize => _verticalCursor,
            CursorType.Pointer => _pointerCursor,
            CursorType.TopLeftResize => _verticalCursor,
            CursorType.TopRightResize => _verticalCursor,
            CursorType.BottomLeftResize => _verticalCursor,
            CursorType.BottomRightResize => _verticalCursor,
            _ => _pointerCursor,
        };
    }

    private void SetCursor(float x, float y)
    {
        Texture2D cursorTexture = GetCursorTexture(x, y);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private Texture2D GetCursorTexture(float x, float y)
    {
        if (x != 0)
            return _horizontalCursor;
        if (y != 0)
            return _verticalCursor;
        return _pointerCursor;
    }
}
