using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    [SerializeField] private Spline _spline;

    private void OnSceneGUI()
    {
        _spline = (Spline) target;

        HandleInput();
        DrawCurve();
    }

    private void DrawCurve()
    {
        // Draw the outline of the spline
        Vector2 lastPoint = _spline[0];
        Handles.color = Color.red;
        for (int i = 1; i < _spline.Count; i++)
        {
            Handles.DrawLine(lastPoint, _spline[i]);
            lastPoint = _spline[i];
        }

        // Draw the spline
        lastPoint = _spline.Count > 0 ? _spline[0] : Vector2.zero;
        Handles.color = Color.green;
        for (int i = 1; i <= _spline.Resolution; i++)
        {
            float t = i / (float) _spline.Resolution;

            Vector2 point = _spline.Evaluate(t);
            Handles.DrawLine(lastPoint, point);
            lastPoint = point;
        }

        // Draw the handles
        Handles.color = Color.white;
        for (int i = 0; i < _spline.Count; i++)
        {
            var newPos = Handles.FreeMoveHandle(_spline[i], Quaternion.identity, .1f, Vector3.zero,
                Handles.CylinderHandleCap);
            _spline.MovePoint(i, newPos);
        }
    }

    private void HandleInput()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            _spline.NewSegment(mousePos);
        }
    }
}