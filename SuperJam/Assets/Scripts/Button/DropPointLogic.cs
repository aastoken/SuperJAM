using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPointLogic : MonoBehaviour
{
    #region Public
    public GameObject door;
    public List<RobotBehaviour> WaitingList;
    public List<RobotBehaviour> CorrectList;
    public List<RobotBehaviour> IncorrectList;
    public bool allowBridgeExit;
    public bool allowDropPointEntrance;
    
    #endregion

    #region Private
    DoorState _currentState;

    bool _correctPath = true;
    float _doorAngle = 90;
    Quaternion _initialRotation;
    Quaternion _targetRotation;
    float _rotationSpeed = 50f;
    float _delta;
    float _t;

    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.IDLE;
        _initialRotation = door.transform.rotation;
        
        
        _targetRotation = Quaternion.AngleAxis(door.transform.rotation.eulerAngles.y + _doorAngle, Vector3.up);
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
    private void ManageQueues()
    {
        if(WaitingList.Count!=0)
        {
            if (_currentState == DoorState.IDLE)
            {
                allowBridgeExit = true; //From here, robot is allowed to move to the door's area (a point in the middle of the square of the door)
                _currentState = DoorState.ROBOT_PASSING;
            }
            else
            {
                allowBridgeExit = false;
                _currentState = DoorState.ROBOT_PASSING;
            }
            

        }

        if (allowBridgeExit && _correctPath && CorrectList.Count == 0)
        {
            allowDropPointEntrance = true;
            //Evaluate is correctPath is true from the robot's behaviour
        }
        else if (allowBridgeExit && !_correctPath && IncorrectList.Count == 0)
        {
            allowDropPointEntrance = true;
            //Evaluate is correctPath is false from the robot's behaviour
        }

    }

    private void MoveDoor()
    {
        if(_currentState != DoorState.ROBOT_PASSING)
        {
            if (_correctPath)
            {
                _t += _rotationSpeed * _delta;
            }
            else
            {
                _t -= _rotationSpeed * _delta;
            }
        }
        

        

        _t = Mathf.Clamp01(_t);

        door.transform.rotation = Quaternion.Slerp(_initialRotation, _targetRotation, _t);

        if (_t <= 0) _correctPath = true;
        else if (_t >= 1) _correctPath = false;

        if (_t >= 1 || _t <= 0) _currentState = DoorState.IDLE;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Robot"))
        {
            _currentState = DoorState.IDLE; //This resets the door after a robot comes through
        }
    }
    #endregion
}