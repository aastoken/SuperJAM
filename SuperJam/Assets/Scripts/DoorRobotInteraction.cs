using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRobotInteraction : MonoBehaviour
{
    #region Private
    private BoxColor _currentDoorColor = BoxColor.BLUE;
    #endregion

    #region Public
    public BoxColor initialDoorColor = BoxColor.BLUE;
    #endregion 


    #region MonoBehaviour 
    void Start()
    {
        _currentDoorColor = initialDoorColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public

    public void ChangeColor(BoxColor c)
    {
        _currentDoorColor = c;
    }

    public BoxColor CurrentColor()
    {
        return _currentDoorColor;
    }

    #endregion
}
