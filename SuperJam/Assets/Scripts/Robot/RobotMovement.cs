using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Move(Vector3 dir)
    {

    }
    #endregion
}
