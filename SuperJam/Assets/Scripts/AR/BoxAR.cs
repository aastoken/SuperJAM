using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAR : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
            collision.gameObject.GetComponent<RobotAR>().score += 1;
            Destroy(gameObject);
        }
    }
}
