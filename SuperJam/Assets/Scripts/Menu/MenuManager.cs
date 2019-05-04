using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenuManager : MonoBehaviour
{
    public Transform boxSpawnPoint;
    public Transform dropZone;
    public Transform objectHandler;
    public GameObject box;
    public float highScore;
    
    

    public enum State
    {
        PICKTHEBOX,
        DROPTHEBOX
    }

    State mainState;

    // Start is called before the first frame update
    void Start()
    {
        mainState = State.PICKTHEBOX;
    }

    void Update()
    {
        StateHandling();
    }

    void TravelToPoint()
    {

        if(mainState == State.PICKTHEBOX)
        {
            GetComponent<RobotMovement>().Move(boxSpawnPoint.position);

            Vector3 distance = boxSpawnPoint.position - transform.position;
            Debug.Log(distance.magnitude);
            if (distance.magnitude < 2.0f)
            {
                GetBox();
                GetComponent<RobotMovement>().Move(dropZone.transform.position);
                mainState = State.DROPTHEBOX;
            }
        }
    }

    void TravelToDropZone()
    {
        if (mainState == State.DROPTHEBOX)
        {
            GetComponent<RobotMovement>().Move(dropZone.position);

            Vector3 distance = dropZone.position - transform.position;
           

            if (distance.magnitude < 2.0f)
            {
                DeleteBox();
                GetComponent<RobotMovement>().Move(boxSpawnPoint.position);
                mainState = State.PICKTHEBOX;
            }
        }
    }

    void GetBox()
    {
        box.SetActive(true);
        box.GetComponent<Rigidbody>().useGravity = false;
        box.transform.parent = objectHandler.transform;
        box.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }


    void DeleteBox()
    {
        box.GetComponent<Rigidbody>().useGravity = true;
        box.transform.parent = null;
        box.SetActive(false);
    }

    void StateHandling()
    {
        switch (mainState)
        {
            case State.PICKTHEBOX:
                TravelToPoint();
                break;

            case State.DROPTHEBOX:
                TravelToDropZone();
                break;
        }
    }
}
