using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    #region Public 
    public float maxSpeed = 5.0f;
    public float acceleration = 3.0f;
    #endregion

    #region Private
    Vector3 velocity;
    #endregion

    #region MonoBehaviour
    /// <summary>
    /// Move to the objective.
    /// </summary>
    /// <param name="objective">Objective.</param>
    public void Move(Vector3 objective)
    {
        /*
        float dt = Time.deltaTime;
        // Direction
        Vector3 dir = objective - transform.position;
        // Distance
        float distance = dir.magnitude;
        // Set target velocity
        Vector3 targetVelocity = dir.normalized * maxSpeed;
        Vector3 offsetVelocity = targetVelocity - velocity;
        offsetVelocity = Vector3.ClampMagnitude(offsetVelocity, acceleration * dt);
        velocity += offsetVelocity;
        velocity.y = 0;
        transform.position += velocity * dt;
        */
        // PONER AQUI TU CODIGO DE IA PATHFINDING 
        GetComponent<NavMeshAgent>().SetDestination(objective);

        // todo: make model look at point where he is going.
    }

    /// <summary>
    /// Check if robot is near the objective instance
    /// </summary>
    /// <returns><c>true</c>, if he near instance, <c>false</c> otherwise.</returns>
    /// <param name="instance">Instance.</param>
    public bool IsHeNearInstance(Vector3 instance)
    {
        Vector3 dist = transform.position - instance;
        return Mathf.Abs(dist.magnitude) < 3f;
    }

    public void SetInstantSpeed(float spd)
    {
        GetComponent<NavMeshAgent>().velocity = Vector3.one * spd;
    }

    public bool IsHeLookingAt(Vector3 instance)
    {
        if (Vector3.Angle(transform.forward, instance - transform.position) < 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
