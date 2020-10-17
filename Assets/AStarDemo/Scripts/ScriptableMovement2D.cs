using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableMovement2D : MonoBehaviour
{
    public float MovementSpeed = 5.0f;

    private Rigidbody2D rb;


    private bool _hasGoal = false;
    public UnityEvent hasGoalChanged;

    public bool hasGoal
    {
        get
        {
            return _hasGoal;
        }

        set
        {
            var shouldInvoke = (_hasGoal != value);
            _hasGoal = value;

            if (shouldInvoke)
                hasGoalChanged.Invoke();
        }
    }

    public Vector2 goal { get; private set; }
    public double distToGoal { get; private set; }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        hasGoalChanged = new UnityEvent();
    }

    private double DistanceSquared(Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude;
    }

    public void SetGoal(Vector2 g, double gDist = 1.0)
    {
        goal = g;
        distToGoal = gDist * gDist; // we use distance squared for optimization
        hasGoal = true;
    }

    public void ClearGoal()
    {
        hasGoal = false;
    }

    private void FixedUpdate()
    {
        if (hasGoal)
        {
            if (DistanceSquared(rb.position, goal) > distToGoal)
            {
                rb.MovePosition(rb.position + ((goal - rb.position).normalized * MovementSpeed * Time.fixedDeltaTime));
            }
            else
            {
                hasGoal = false;
            }
        }
    }
}
