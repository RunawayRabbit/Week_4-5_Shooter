
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(PlayerWeaponSlot))]
public class PlayerWeaponSlotEditor : Editor
{
    private ArcHandle _arcHandle = new ArcHandle();

    private void OnEnable()
    {
        _arcHandle.SetColorWithRadiusHandle(Color.yellow, 0.12f);
        _arcHandle.angle = ((PlayerWeaponSlot) target).turningArc;
        _arcHandle.radius = 1.0f;
        _arcHandle.angleHandleColor = Color.green;
        _arcHandle.radiusHandleColor = default;
    }

    public void OnSceneGUI()
    {
        var weaponSlot = (PlayerWeaponSlot) target;
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
                ResetMaxRotation(weaponSlot, _arcHandle.angle);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _arcHandle.angle = ((PlayerWeaponSlot) target).turningArc;
        ResetMaxRotation((PlayerWeaponSlot) target, ((PlayerWeaponSlot) target).turningArc);
    }

    private void ResetMaxRotation(WeaponSlot weaponSlot, float degrees)
    {
        var newAngle = degrees * Mathf.Deg2Rad;
        weaponSlot.maxRotation = new Vector3(Mathf.Sin(newAngle), 0.0f, Mathf.Cos(newAngle));
    }
}