
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(WeaponSlot))]
public class WeaponSlotEditor : Editor
{
    private ArcHandle _arcHandle = new ArcHandle();

    private void OnEnable()
    {
        _arcHandle.SetColorWithRadiusHandle(Color.yellow, 0.12f);
        _arcHandle.angle = ((WeaponSlot) target).turningArc;
        _arcHandle.radius = 1.0f;
        _arcHandle.angleHandleColor = Color.green;
        _arcHandle.radiusHandleColor = default;
    }

    public void OnSceneGUI()
    {
        var weaponSlot = (WeaponSlot) target;
        var transform = weaponSlot.transform;

        Vector3 handleDirection = transform.forward;
        Vector3 handleNormal = Vector3.up;
        Matrix4x4 handleMatrix = Matrix4x4.TRS(
            transform.position,
            Quaternion.LookRotation(handleDirection, handleNormal),
            Vector3.one);

        using (new Handles.DrawingScope(handleMatrix))
        {
            EditorGUI.BeginChangeCheck();
            _arcHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(weaponSlot, "Changed weapon slot arc.");
                weaponSlot.turningArc = _arcHandle.angle;
                var newAngle = _arcHandle.angle * Mathf.Deg2Rad;
                weaponSlot.maxRotation = new Vector3(Mathf.Sin(newAngle), 0.0f, Mathf.Cos(newAngle));
            }
        }
    }
}