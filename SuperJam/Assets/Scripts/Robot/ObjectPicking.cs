using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicking : MonoBehaviour
{

    public GameObject targetObject;
    Vector3 distanceToObject;
    float distanceValue;


    //STATES

    public enum State
    {
        IDLE,
        TRAVELLING,
        REACHED,
        PICKEDUP,
        DROPPING
    }

    State mainState;




    // GETTERS AND SETTERS
    public void SetTarget(GameObject go)
    {
        targetObject = go;
    }

    //************************ ACTIONS *********************************//


    //Delete after getting a pathfinding
    void TravelToObject()
    {
        if (targetObject != null)
        {
            distanceToObject = targetObject.transform.position - transform.position;
            distanceValue = distanceToObject.magnitude;

            // TODO Use this in RobotMovement.
            transform.Translate((distanceToObject + new Vector3(0.0f, 0.5f, 0.0f)) * Time.deltaTime * 2.0f);

            if (distanceValue < 1.5f)
            {
                mainState = State.REACHED;
            }
        }            

    }

    public void PickUpObject()
    {
        targetObject.transform.SetParent(transform);
        targetObject.transform.position = transform.position + distanceToObject.normalized;
        targetObject.GetComponent<Rigidbody>().useGravity = false;
        mainState = State.PICKEDUP;
    }

    public void DropObject()
    {
        targetObject.transform.SetParent(null);
        targetObject.GetComponent<Rigidbody>().useGravity = true;
        targetObject = null;
        mainState = State.IDLE;
    }


    //***********************************************************************************************//



    void StateHandling()
    {
        switch(mainState)
        {
            case State.IDLE:
                break;

            case State.TRAVELLING:
                TravelToObject();
                break;

            case State.REACHED:
                PickUpObject();
                break;

            case State.PICKEDUP:
                break;

            case State.DROPPING:
                DropObject();
                break;
       
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainState = State.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mainState = State.TRAVELLING;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && mainState == State.PICKEDUP)
        {
            mainState = State.DROPPING;
        }

        StateHandling();
        
    }
}
