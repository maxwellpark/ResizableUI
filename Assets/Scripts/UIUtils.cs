using UnityEngine;

public static class UIUtils
{
    public static void GetCursorPosition(RectTransform rect,
        out Vector3 parentPos,
        out Vector3 localPos)
    {
        Ray ray = new Ray(Input.mousePosition, Vector3.forward);
        localPos = rect.InverseTransformPoint(ray.origin);
        Vector3 direction = rect.InverseTransformDirection(ray.direction);

        localPos -= direction * (localPos.z / direction.z);
        parentPos = rect.parent.InverseTransformPoint(rect.TransformPoint(localPos));
        localPos += rect.pivot.x * rect.rect.width * Vector3.right + rect.pivot.y * rect.rect.height * Vector3.up;
    }
}
