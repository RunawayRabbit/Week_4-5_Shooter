using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Arena))]
public class ArenaEditor : Editor
{

    private void OnSceneGUI()
    {
        var arena = (Arena) target;
        var transform = arena.transform;

        arena.horizontalArena = ResizeArena(transform, arena.horizontalArena, Arena.Mode.Horizontal);
        arena.vertcalArena = ResizeArena(transform, arena.vertcalArena, Arena.Mode.Vertical);
    }

    private static Vector2 ResizeArena(Transform transform, Vector2 arena, Arena.Mode mode)
    {
        Color fillCol = (mode == Arena.Mode.Horizontal) ? Handles.yAxisColor : Handles.zAxisColor;
        Handles.color = fillCol;
        
        Vector2 halfSize = new Vector2(arena.x * 0.5f, arena.y * 0.5f);
        Vector3[] corners = GenerateCorners(halfSize, mode);

        Handles.matrix =
            transform.localToWorldMatrix;
        var transparentColor = new Color(fillCol.r, fillCol.g, fillCol.b, 0.15f);
        Handles.DrawSolidRectangleWithOutline(corners, transparentColor, fillCol);

        Vector3[] handlePoints = GenerateHandlePoints(halfSize, mode);

        float capSize = 0.25f;
        float snap = 0.1f;

        Vector3 basisX = Vector3.right;
        Vector3 basisY = Vector3.forward + Vector3.up;

        var newSize = new Vector3(arena.x, arena.y, arena.y);
        newSize -= Handles.Slider(handlePoints[0], -basisX,
            capSize, Handles.SphereHandleCap, snap) - handlePoints[0];
        newSize += Handles.Slider(handlePoints[1], basisX,
            capSize, Handles.SphereHandleCap, snap) - handlePoints[1];
        newSize += Handles.Slider(handlePoints[2], basisY,
            capSize, Handles.SphereHandleCap, snap) - handlePoints[2];
        newSize -= Handles.Slider(handlePoints[3], -basisY,
            capSize, Handles.SphereHandleCap, snap) - handlePoints[3];
        
        Vector2 returnValue;
        if(mode == Arena.Mode.Horizontal)
            returnValue = new Vector2(newSize.x, newSize.z);
        else
            returnValue = new Vector2(newSize.x, newSize.y);
        
        return Vector2.Max(returnValue, Vector2.zero);
    }

    private static Vector3[] GenerateHandlePoints(Vector2 halfSize, Arena.Mode mode)
    {
        Vector3[] handlePoints = new Vector3[4];

        handlePoints[0] = new Vector3(-halfSize.x, 0.0f, 0.0f); // Left
        handlePoints[1] = new Vector3(halfSize.x, 0.0f, 0.0f); // Right
        
        // Top
        handlePoints[2] = new Vector3(0.0f,
            (mode == Arena.Mode.Horizontal ? 0.0f : halfSize.y),
            (mode == Arena.Mode.Horizontal ? halfSize.y : 0.0f));
        // Bottom
        handlePoints[3] = new Vector3(0.0f,
            (mode == Arena.Mode.Horizontal ? 0.0f : -halfSize.y),
            (mode == Arena.Mode.Horizontal ? -halfSize.y : 0.0f));

        return handlePoints;
    }

    private static Vector3[] GenerateCorners(Vector2 halfSize, Arena.Mode mode)
    {
        Vector3[] corners = new Vector3[4];
        // Top Left
        corners[0] = new Vector3(-halfSize.x,
            (mode == Arena.Mode.Horizontal ? 0.0f : halfSize.y),
            (mode == Arena.Mode.Horizontal ? halfSize.y : 0.0f));

        // Top Right
        corners[1] = new Vector3(halfSize.x,
            (mode == Arena.Mode.Horizontal ? 0.0f : halfSize.y),
            (mode == Arena.Mode.Horizontal ? halfSize.y : 0.0f));

        // Bottom Right
        corners[2] = new Vector3(halfSize.x,
            (mode == Arena.Mode.Horizontal ? 0.0f : -halfSize.y),
            (mode == Arena.Mode.Horizontal ? -halfSize.y : 0.0f));

        // Bottom Left
        corners[3] = new Vector3(-halfSize.x,
            (mode == Arena.Mode.Horizontal ? 0.0f : -halfSize.y),
            (mode == Arena.Mode.Horizontal ? -halfSize.y : 0.0f));

        return corners;
    }
}
