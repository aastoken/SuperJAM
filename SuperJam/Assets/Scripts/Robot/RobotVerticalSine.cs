using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVerticalSine : MonoBehaviour
{
    void Update()
    {
        VerticalSineMovement();
    }

    /// <summary>
    /// This fucking method makes the robot go up and down as my dick
    /// </summary>
    void VerticalSineMovement()
    {
        float dt = Time.deltaTime;
        float amplitude = 10.0f;
        float period = 2 * Mathf.PI;

        float sine = amplitude * Mathf.Sin(period * dt);

        transform.position += new Vector3(0.0f, Mathf.Abs(sine), 0.0f);
    }
}
