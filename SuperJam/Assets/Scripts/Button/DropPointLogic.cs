using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointsDrop
{
    QUEUE, WAITSTATION, DROPRIGHT, DROPWRONG, NOTHING
}

public class DropPointLogic : MonoBehaviour
{
    #region Public
    public Transform door;
    public Transform buttonDeco;
    public List<RobotBehaviour> WaitingList;
    public Transform []paths;
    public RobotBehaviour CorrectBot;
    public RobotBehaviour IncorrectBot;
    public bool allowBridgeExit;
    public bool allowDropPointEntrance;
    
    #endregion

    #region Private
    DoorState _currentState;

    RobotBehaviour _tempTriggerBot;
    bool _correctPath = true;
    float _doorAngle = 90;
    Quaternion _initialRotation;
    Quaternion _targetRotation;
    float _rotationSpeed = 10f;
    float _delta;
    float _t;
    bool _allowButtonRot = false;

    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.IDLE;
        _initialRotation = door.transform.localRotation;
        
        
        _targetRotation = Quaternion.AngleAxis(door.transform.localRotation.eulerAngles.y - _doorAngle, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        _delta = Time.deltaTime;
        Debug.Log("DOOR ANGLE: " + _doorAngle + " , TARGET: " + _targetRotation);

        if (_currentState == DoorState.MOVING)
            MoveDoor();

        ManageQueues();
    }

    public void AddToQueue(RobotBehaviour robotiyo)
    {
        WaitingList.Add(robotiyo);
    }

    public bool CanIGo(int id)
    {
        return WaitingList[0].gameObject.GetInstanceID() == id;
    }

    public Vector3 GetPoint(PointsDrop desiredDrop)
    {
        int desiredInt = (int)desiredDrop;
        
        return paths[desiredInt].position;
    }

    public PointsDrop WhatDrop(int id)
    {
       if (CorrectBot.gameObject.GetInstanceID() == id)
       {
            return PointsDrop.DROPRIGHT;
       }
       else if (IncorrectBot.gameObject.GetInstanceID() == id)
       {
            return PointsDrop.DROPWRONG;
       }
       else
        {
            Debug.LogError("Vaya, al parecer te has equivocado en la máquina de estados... gilipollas...");
            return PointsDrop.NOTHING;
        }
    }

    private void OnMouseDown()
    {
        if (_currentState == DoorState.IDLE)
            _currentState = DoorState.MOVING;

        _allowButtonRot = true;
    }
    #endregion

    #region Methods
    private void ManageQueues()
    {
        if(WaitingList.Count!=0)
        {
            if (_currentState == DoorState.IDLE && _correctPath && !CorrectBot)
            {
                allowDropPointEntrance = true;
                CorrectBot = _tempTriggerBot;
                _currentState = DoorState.ROBOT_PASSING;
            }
            else if (_currentState == DoorState.IDLE && !_correctPath && !IncorrectBot)
            {
                allowDropPointEntrance = true;
                IncorrectBot = _tempTriggerBot;
                _currentState = DoorState.ROBOT_PASSING;
            }
            

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

    private void MoveDecoButton()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        //allowBridgeExit = false;
        if(other.gameObject.CompareTag("Robot"))
        _tempTriggerBot = other.gameObject.GetComponent<RobotBehaviour>();

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Robot"))
        {
            _currentState = DoorState.IDLE; //This resets the door after a robot comes through
        }
        _tempTriggerBot = null;
    }
    #endregion
}