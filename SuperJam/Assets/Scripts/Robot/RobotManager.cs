using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    #region Public

    #endregion

    #region Private
    private RobotState _currentState = RobotState.SEARCH;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        
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

    }
    #endregion
}
