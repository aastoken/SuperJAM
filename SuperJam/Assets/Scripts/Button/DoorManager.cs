using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    GameObject door1;
    GameObject door2;
    GameObject door3;
    GameObject door4;

    // Start is called before the first frame update
    void Awake()
    {
        door1 = GameObject.Find("Door1");
        door2 = GameObject.Find("Door2");
        door3 = GameObject.Find("Door3");
        door4 = GameObject.Find("Door4");

        door1.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 270.0f, 0.0f));
        door2.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
        door3.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
        door4.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 360.0f, 0.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
