using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor
{
    private BezierSpline _path;
    private Transform _targetTransform;
    private Quaternion _handleRotation;

    private void OnSceneGUI()
    {
        _path = (BezierSpline) target;
        _targetTransform = _path.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            _targetTransform.rotation : Quaternion.identity;
        
        if(_path.points.Length == 0) return;
        
        for (int i = 0; i < _path.points.Length; i++)
        {
            Vector3 worldSpacePoint = _targetTransform.TransformPoint(_path.points[i]);
            
            var style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 20;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            Handles.Label(worldSpacePoint, i.ToString(), style);
            
            EditorGUI.BeginChangeCheck();
            
            var newPoint = Handles.DoPositionHandle(worldSpacePoint,_handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_path, "Moved EnemyPath Point");
                EditorUtility.SetDirty(_path);
                _path.points[i] = _targetTransform.InverseTransformPoint(newPoint);
                _path.CalculatePoints();
            }
        }
        DrawBezierCurve();
    }

    private void DrawBezierCurve()
    {
        for (int i = 0, j = 0;
            i < _path.anchorPoints.Length -1 ;
            i++, j +=  2)
        {
            Handles.DrawBezier(
                _targetTransform.TransformPoint(_path.anchorPoints[i]), 
                _targetTransform.TransformPoint(_path.anchorPoints[i + 1]),
                _targetTransform.TransformPoint(_path.controlPoints[j]), 
                _targetTransform.TransformPoint(_path.controlPoints[j + 1]),
                Color.white, Texture2D.whiteTexture, 2.5f);
        }
    }
}