using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    [SerializeField, HideInInspector] private List<Vector2> _points;
    public int Resolution;

    private void Awake()
    {
        if (_points == null)
        {
            Vector2 center = transform.position;
            _points = new List<Vector2>
            {
                center + Vector2.left,
                center + Vector2.left + Vector2.up,
                center + Vector2.right + Vector2.down,
                center + Vector2.right
            };
        }
    }

    public Vector2 this[int i]
    {
        get { return _points[i]; }
    }

    public int Count
    {
        get { return _points.Count; }
    }

    public void MovePoint(int i, Vector2 position)
    {
        _points[i] = position;
    }

    public void NewSegment(Vector2 position)
    {
        _points.Add(position);
    }
    
    public Vector2 Evaluate(float t)
    {
        var n = Count - 1;

        Vector2 sum = Vector2.zero;
        for (int i = 0; i <= n; i++)
        {
            var binomial = Binomial(n, i);
            sum += (binomial * Mathf.Pow(1 - t, n - i) *
                    Mathf.Pow(t, i)) * this[i];
        }

        return sum;
    }

    private float Binomial(int n, int k)
    {
        float c = 1;
        for (int i = 0; i < k; i++)
            c = c * (n - i) / (i + 1);
        return c;
    }
}