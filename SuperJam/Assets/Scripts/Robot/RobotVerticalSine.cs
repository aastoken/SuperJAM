using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVerticalSine : MonoBehaviour
{
    void FixedUpdate()
    {
        Sine();
        //Sine();
    }

    /// <summary>
    /// This fucking method makes the robot go up and down as my dick
    /// </summary>
    public Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        v.y = Mathf.Sin(Mathf.PI * (x / 2 + z / 2 + t / 2)) + 6;
        v.z = z;
        return v;

    }

    void Sine()
    {


        transform.position = Sine2DFunction(transform.position.x, transform.position.z, Time.deltaTime);
        //yield return new WaitForSeconds(3f);
        
        //yield return new WaitForSeconds(0.5f);
    }
}
