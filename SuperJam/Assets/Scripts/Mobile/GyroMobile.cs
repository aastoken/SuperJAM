using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Input.gyro.enabled = false;
        if (IsMobile()) Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMobile())
        {
            float rotx = Input.gyro.rotationRate.x;
            float roty = Input.gyro.rotationRate.y;
            //Vector3 gyroEulerAngles = gyroCurrentState.eulerAngles;
            transform.eulerAngles = new Vector3(rotx, roty, 0);
        }
    }

    bool IsMobile()
    {
        return Application.isMobilePlatform;
    }
}
