using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyPath))]
public class EnemyPathEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyPath path = (EnemyPath) target;
        Transform targetTransform = path.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            targetTransform.rotation : Quaternion.identity;
        
        for (int i = 0; i < path.points.Length; i++)
        {
            Vector3 worldSpacePoint = path.transform.TransformPoint(path.points[i]);
            
            var style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 20;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            Handles.Label(worldSpacePoint, i.ToString(), style);
            
            EditorGUI.BeginChangeCheck();
            
            var newPoint = Handles.DoPositionHandle(worldSpacePoint,handleRotation);
            //Handles.SphereHandleCap(0, worldSpacePoint, Quaternion.identity, 0.03f, EventType.Repaint);
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Moved EnemyPath Point");
                EditorUtility.SetDirty(path);
                path.points[i] = targetTransform.InverseTransformPoint(newPoint);
                path.CalculatePoints();
            }
        }

        
        Handles.color = Color.blue;
        foreach (var point in path.controlPoints)
        {
            Handles.SphereHandleCap(0, path.transform.TransformPoint(point),
                Quaternion.identity, 0.02f, EventType.Repaint);
        }
        
        Handles.color = Color.green;
        foreach (var point in path.anchorPoints)
        {
            Handles.SphereHandleCap(0, path.transform.TransformPoint(point),
                Quaternion.identity, 0.02f, EventType.Repaint);
        }
    }
}