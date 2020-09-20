using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyPath))]
public class EnemyPathEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyPath path = (EnemyPath) target;
        
        Handles.color = Color.red;
        foreach (var point in path.points)
        {
            Handles.SphereHandleCap(0, point, Quaternion.identity, 0.03f, EventType.Repaint);
        }
        
        Handles.color = Color.blue;
        foreach (var point in path.controlPoints)
        {
            Handles.SphereHandleCap(0, point, Quaternion.identity, 0.02f, EventType.Repaint);
        }
        
        Handles.color = Color.green;
        foreach (var point in path.anchorPoints)
        {
            Handles.SphereHandleCap(0, point, Quaternion.identity, 0.02f, EventType.Repaint);
        }
    }
}