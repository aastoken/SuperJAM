using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    #region Public
    public GameObject door;
    #endregion

    #region Private
    DoorSwitch _currentSwitch; //This is for knowing if the door is closing the "correct" (tick) path or the "wrong" (cross) path.
    DoorState _currentState;

    float _doorAngle;
    float _nextRotation;
    float _targetRotation;
    float _rotationSpeed;
    float _delta;

    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.IDLE;
        _currentSwitch = DoorSwitch.CORRECT;
        _doorAngle = door.transform.rotation.eulerAngles.y;
        _rotationSpeed = 40.0f;
        _targetRotation = _doorAngle + 90.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _delta = Time.deltaTime;
        Debug.Log("DOOR ANGLE: " + _doorAngle + " , TARGET: " + _targetRotation);

        if (_currentState == DoorState.MOVING)
            MoveDoor();

    }

    private void OnMouseDown()
    {
        if (_currentState == DoorState.IDLE)
            _currentState = DoorState.MOVING;
    }
    #endregion

    #region Methods
    void MoveDoor()
    {

        //ROTATE TOWARDS THE WRONG PATH;
        if (_doorAngle < _targetRotation && _currentSwitch == DoorSwitch.CORRECT)
        {
            door.transform.Rotate(Vector3.up, _rotationSpeed * _delta);
        }

        //ROTATE TOWARDS THE CORRECT PATH
        if (_doorAngle > _targetRotation && _currentSwitch == DoorSwitch.WRONG)
        {
            door.transform.Rotate(Vector3.up, -1 * _rotationSpeed * _delta);
        }

        _nextRotation = _doorAngle + _rotationSpeed * _delta;
        _doorAngle = door.transform.rotation.eulerAngles.y;

        if ( _nextRotation > _targetRotation )
        {
            _currentState = DoorState.IDLE;

            switch ( _currentSwitch )
            {
                case DoorSwitch.CORRECT:
                    _targetRotation = _doorAngle + 90.0f;
                    _currentSwitch = DoorSwitch.WRONG;
                    break;

                case DoorSwitch.WRONG:
                    _targetRotation = _doorAngle - 90.0f;
                    _currentSwitch = DoorSwitch.CORRECT;
                    break;

            }
        }


    }
    #endregion
}