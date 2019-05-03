using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    #region Public
    public GameObject testBox = null;
    #endregion

    #region Private
    private RobotState _currentState = RobotState.SEARCH;
    private GameObject _currentBoxTarget = null;
    private BoxManager _currentBoxTargetManager = null;
    private RobotMovement _rm;
    private ObjectPicking _op;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        _rm = GetComponent<RobotMovement>();
        if (_rm == null)
        {
            Debug.LogError("ERROR! Set the RobotMovement Script in the prefab.");
        }
        _op = GetComponent<ObjectPicking>();
        if (_op == null)
        {
            Debug.LogError("ERROR! Set the ObjectPicking Script in the prefab.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    #endregion

    #region Methods
    void Control()
    {
        switch(_currentState)
        {
            case RobotState.SEARCH:
                // With the probability script here we will find the box, when
                // the box is found set state to GO
                _currentBoxTarget = testBox;
                _currentBoxTargetManager = testBox.GetComponent<BoxManager>();
                _currentState = RobotState.GO;
                break;
            case RobotState.GO:
                HandleGo();
                break;
            case RobotState.TAKEBOX:
                HandleTakeBox();
                break;
            case RobotState.WITHBOX:
                // With box function
                break;
            case RobotState.LEAVEBOX:
                break;
            case RobotState.WAIT:
                break;
        }
    }
       
    /// <summary>
    /// Handles the go state.
    /// </summary>
    void HandleGo()
    {
        if (_currentBoxTargetManager == null)
        {
            Debug.LogWarning("Current box target is not found? Probably an error finding it?");
            _currentState = RobotState.SEARCH;
            return;
        }
        if (_currentBoxTargetManager.GetState() == BoxState.PICKED)
        {
            _currentState = RobotState.SEARCH;
            return;
        }
        _rm.Move(_currentBoxTarget.transform.position);
        if (_rm.IsHeNearInstance(_currentBoxTarget.transform.position)) {
            _currentState = RobotState.TAKEBOX;
            return;
        }
    }

    /// <summary>
    /// Handles the take box state.
    /// </summary>
    void HandleTakeBox()
    {
        if (_currentBoxTargetManager.GetState() == BoxState.PICKED)
        {
            _currentState = RobotState.SEARCH;
            return;
        }
        _op.SetTarget(_currentBoxTarget);
        _op.PickUpObject();
        _currentState = RobotState.WITHBOX;
        return;
    }

    #endregion
}
