using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    DoorSwitch _currentSwitch; //This is for knowing if the door is closing the "correct" (tick) path or the "wrong" (cross) path.
    DoorState _currentState;
    public GameObject door;
    float _doorAngle;
    float _doorTargetAngle = 90;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.IDLE;
        _doorAngle = door.transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (_currentState == DoorState.MOVING)
            MoveDoor();

    }

    private void OnMouseDown()
    {
        if (_currentState == DoorState.IDLE)
            _currentState = DoorState.MOVING;
    }

    void MoveDoor()
    {


    }

}
