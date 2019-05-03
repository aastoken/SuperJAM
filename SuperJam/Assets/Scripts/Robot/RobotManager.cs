using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    #region Public

    #endregion

    #region Private
    private RobotState _currentState = RobotState.SEARCH;
    private GameObject _currentBoxTarget = null;
    private BoxManager _currentBoxTargetManager = null;
    private RobotMovement _rm;
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
                break;
            case RobotState.GO:
                HandleGo();
                break;
            case RobotState.TAKEBOX:
                break;
            case RobotState.WITHBOX:
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
        }
        if (_currentBoxTargetManager.GetState() == BoxState.PICKED)
        {
            _currentState = RobotState.SEARCH;
        }
    }

    #endregion
}
