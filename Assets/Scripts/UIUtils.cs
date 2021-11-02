using UnityEngine;

public static class UIUtils
{
    public static void GetCursorPosition(RectTransform rect,
        out Vector3 parentPos,
        out Vector3 localPos,
        bool isResizing = false)
    {
        Ray ray = new Ray(Input.mousePosition, Vector3.forward);
        localPos = rect.InverseTransformPoint(ray.origin);
        Vector3 direction = rect.InverseTransformDirection(ray.direction);
        Vector3 point = ray.origin + ray.direction * ((rect.position.z - ray.origin.z) / ray.direction.z);

        if (!isResizing)
            parentPos = rect.parent.InverseTransformPoint(point);
        else
            parentPos = rect.parent.InverseTransformPoint(rect.TransformPoint(localPos));

        localPos -= direction * (localPos.z / direction.z);
        localPos += rect.pivot.x * rect.rect.width * Vector3.right + rect.pivot.y * rect.rect.height * Vector3.up;
    }
}
