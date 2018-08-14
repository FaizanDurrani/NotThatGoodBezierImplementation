using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: UNFINISHED
public class Curve : MonoBehaviour
{
    [SerializeField, HideInInspector] private List<Vector2> _points;
    public float Resolution;

    public Vector2 this[int i]
    {
        get { return _points[i]; }
    }

    public int Count
    {
        get { return _points.Count; }
    }

    private void Awake()
    {
        if (_points == null)
        {
            Vector2 center = transform.position;
            _points = new List<Vector2>
            {
                center + Vector2.left,
                center + Vector2.left + Vector2.up,
                center + Vector2.right + Vector2.up,
                center + Vector2.right
            };
        }
    }

    public int NumberOfSegments
    {
        get { return (_points.Count - 4) / 3 + 2; }
    }

    public List<Vector2> PointsInSegment(int i)
    {
        return new List<Vector2> {_points[i * 3], _points[i * 3 + 1], _points[i * 3 + 2], _points[i * 3 + 3]};
    }

    public void NewSegment(Vector2 position)
    {
        var segment = _points[Count - 1];
        var dir = _points[Count - 2] - segment;
        var distance = Vector2.Distance(segment, _points[Count - 2]);
        _points.Add(segment + (-dir.normalized * distance));

        dir = _points[Count - 1] - position;
        _points.Add(position + (-dir.normalized * 1f));
        _points.Add(position);
    }

    public void MovePoint(int i, Vector2 position)
    {
        var mod = i % 3;
        if (mod == 0)
        {
            // Moving a segment

            var diff = position - _points[i];

            if (i < _points.Count - 1)
                _points[i + 1] += diff;
            if (i - 1 > 0)
                _points[i - 1] += diff;
        }
        else
        {
            // Moving a control node
            if (mod == 2 && i + 2 < Count)
            {
                var segment = _points[i + 1];
                var dir = position - segment;
                var distance = Vector2.Distance(segment, _points[i + 2]);
                _points[i + 2] = segment + (-dir.normalized * distance);
            }
            else if (mod == 1 && i - 2 >= 0)
            {
                var segment = _points[i - 1];
                var dir = position - segment;
                var distance = Vector2.Distance(segment, _points[i - 2]);
                _points[i - 2] = segment + (-dir.normalized * distance);
            }
        }

        _points[i] = position;
    }
}