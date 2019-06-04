using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PointsDrop
{
    QUEUE, WAITSTATION, DROPRIGHT, DROPWRONG, VENTCORRECT, VENTINCORRECT, EXITCORRECT, EXITINCORRECT, NOTHING 
}

public class DropPointLogic : MonoBehaviour
{
    #region Public
    public Transform door;
    public Transform buttonDeco;
    public DoorRobotInteraction DRI; 
    public List<RobotBehaviour> WaitingList;
    public Transform []paths;
    public RobotBehaviour CorrectBot;
    public RobotBehaviour IncorrectBot;
    public bool allowDropPointEntrance;
    public float buttonRotationSpeed = 50f;
    public RobotBehaviour waitZone = null;

    #endregion

    #region Private
    DoorState _currentState;
    // To change the color
    Transform dropperBody;
    RobotBehaviour _tempTriggerBot;
    bool _correctPath = true;
    float _doorAngle = -90;
    Quaternion _initialRotation;
    Quaternion _targetRotation;
    float _rotationSpeed = 10f;
    float _delta;
    float _t;
    bool _allowButtonRot = false;
    float _timer = 0f;
    bool _canUseAxis = true;

    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.IDLE;
        _initialRotation = door.transform.rotation;
        dropperBody = transform.GetChild(0);
        
        
        _targetRotation = Quaternion.AngleAxis(door.transform.rotation.eulerAngles.y + _doorAngle, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        // inputs
        float inputRed = Input.GetAxis("Red");
        float inputBlue = Input.GetAxis("Blue");
        float inputGreen = Input.GetAxis("Green");
        float inputYellow = Input.GetAxis("Yellow");
        CheckInputs(inputRed, inputYellow, inputBlue, inputGreen);
        _delta = Time.deltaTime;


        if (_currentState == DoorState.MOVING)
        {
            MoveDoor();            
        }
        MoveDecoButton();
        SetHighlightColor();
        ManageQueues();
    }

    public void SetHighlightColor()
    {
        dropperBody.GetComponent<MeshRenderer>().materials[2].color = DRI.CurrentRGBColor();
    }

    public void AddToQueue(RobotBehaviour robotiyo)
    {
        WaitingList.Add(robotiyo);
    }

    public bool CanIGo(int id)
    {
        return WaitingList[0].gameObject.GetInstanceID() == id;
    }

    public Vector3 GetPoint(PointsDrop desiredPoint)
    {
        int desiredInt = (int)desiredPoint;
        
        return paths[desiredInt].position;
    }

    public void SetBotInDropZone(RobotBehaviour rb, PointsDrop pd)
    {
        WaitingList.RemoveAt(0);
        if (pd == PointsDrop.DROPRIGHT)
        {
            CorrectBot = rb;
        }
        else
        {
            IncorrectBot = rb;
        }
    }

    public PointsDrop WhatDrop(int id)
    {
        // if (CorrectBot) Debug.Log("righttt");
       if (_correctPath && !CorrectBot)
       {
            return PointsDrop.DROPRIGHT;
       }
       if (!_correctPath && !IncorrectBot)
       {
            return PointsDrop.DROPWRONG;
       }
       return PointsDrop.NOTHING;
    }

    public bool WhereIsHe()
    {
        if (CorrectBot)
        {
            return true;
        }
        if (IncorrectBot)
        {
            return false;
        }
        return false;
    }

    private void OnMouseDown()
    {
        // Debug.Log("WORKS");
        SoundManager.instance.openDoor(gameObject.GetComponent<AudioSource>());
        Debug.LogWarning("aaaa!)");
        if (_currentState == DoorState.IDLE)
            _currentState = DoorState.MOVING;

        _allowButtonRot = true;
    }
    #endregion

    #region Methods
    private void CheckInputs(float ir, float iy, float ib, float ig)
    {
        if (_canUseAxis)
        {
            BoxColor color = DRI.initialDoorColor;
            bool used = true;
            if (color == BoxColor.RED && Mathf.Abs(ir) > 0.01f)
            {
                ExecuteClick();
            }
            else if (color == BoxColor.GREEN && Mathf.Abs(ig) > 0.01f)
            {
                ExecuteClick();
            }
            else if (color == BoxColor.YELLOW && Mathf.Abs(iy) > 0.01f)
            {
                ExecuteClick();
            }
            else if (color == BoxColor.BLUE && Mathf.Abs(ib) > 0.01f)
            {
                ExecuteClick();
            }
            else used = false;
            if (used) { _canUseAxis = false; StartCoroutine(StartCanInput()); }

        }

    }


    private void ManageQueues()
    {
        if (waitZone)
        {
           // Debug.Log("uwu");
            if (_currentState == DoorState.IDLE && _correctPath && !CorrectBot)
            {
                //Debug.LogWarning("Yes");
                allowDropPointEntrance = true;
                CorrectBot = _tempTriggerBot;
                _currentState = DoorState.ROBOT_PASSING;
            }
            else if (_currentState == DoorState.IDLE && !_correctPath && !IncorrectBot)
            {
               // Debug.LogWarning("No");
                allowDropPointEntrance = true;
                IncorrectBot = _tempTriggerBot;
                _currentState = DoorState.ROBOT_PASSING;
            }
        }
    }

    public void SetIDLE()
    {
        _currentState = DoorState.IDLE;
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
        Quaternion temp = Quaternion.identity;
        bool hasBeenClicked = false;
        if (_allowButtonRot)
        {
            _timer = 0.5f;
            hasBeenClicked = true;
            _allowButtonRot = false;
        }
        _timer -= Time.deltaTime;
        _timer = Mathf.Clamp01(_timer);
        if (_timer > 0)
        {
            buttonDeco.transform.Rotate(Vector3.up * 400 * Time.deltaTime);
            temp = buttonDeco.transform.rotation;
        }
        else if (hasBeenClicked && _timer <= 0)
        {
            buttonDeco.transform.rotation = Quaternion.Slerp(temp, transform.rotation, Time.deltaTime * 2f);
            hasBeenClicked = false;
        }
    }

    private IEnumerator StartCanInput()
    {
        yield return new WaitForSeconds(0.5f);
        _canUseAxis = true;

    }

    public static float QuadraticInOut(float k)
    {
        if ((k *= 2f) < 1f) return 0.5f * k * k;
        return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
    }



    

    private void OnTriggerStay(Collider other)
    {
        //allowBridgeExit = false;
        // Debug.Log("ontrigger");
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

    public void ExecuteClick()
    {
        
        SoundManager.instance.openDoor(gameObject.GetComponent<AudioSource>());
        if (_currentState == DoorState.IDLE)
            _currentState = DoorState.MOVING;

        _allowButtonRot = true;
    }
    #endregion
}