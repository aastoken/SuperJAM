using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    // This parameters will be get by the size of the terrain the robots will be playing, their values here are placeholders
    public float arenaTamX = 10.0f;
    public float arenaTamZ = 10.0f;

    private Vector3 _currentPositionWander = Vector3.one * -1;
    private bool _finishedMovingWander = false;
    private float levitatingSpeed = 10.0f;

    public void Update()
    {
        float dt = Time.deltaTime;
        transform.position += new Vector3(0.0f, 0.2f * Mathf.Sin(levitatingSpeed * dt), 0.0f);
    }

    /// <summary>
    /// This method handles the wander movement of the robot (implement inside SEARCH state)
    /// </summary>
    void HandleWander()
    {
        if (_finishedMovingWander)
        {
            Vector2 nextCoords = new Vector2(Random.Range(-arenaTamX, arenaTamX), Random.Range(-arenaTamZ, arenaTamZ));
            _currentPositionWander = new Vector3(nextCoords.x, 1.0f, nextCoords.y);
            _finishedMovingWander = false;
        }
        //_rm.Move(_currentPositionWander);

        //if (_rm.IsNearInstance(_currentPositionWander))
        {
            _finishedMovingWander = true;
        }
    }

    void LevitationMovement()
    {
        
    }
}
