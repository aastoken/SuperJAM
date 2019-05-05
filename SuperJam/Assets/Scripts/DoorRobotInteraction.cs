using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRobotInteraction : MonoBehaviour
{
    #region Private
    private BoxColor _currentDoorColor = BoxColor.BLUE;
    private GameManager gm;
    #endregion

    #region Public
    public BoxColor initialDoorColor = BoxColor.BLUE;
    public DropPointLogic dropper;
    #endregion 


    #region MonoBehaviour 
    void Start()
    {
        _currentDoorColor = initialDoorColor;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

    public Color CurrentRGBColor()
    {
        return gm.colors[(int)_currentDoorColor];
    }

    public DropPointLogic Droper()
    {
        return dropper;
    }

    /// <summary>
    /// Checks if the robot is right, and cleans the correspondent field
    /// </summary>
    /// <returns><c>true</c>, if robot right was used, <c>false</c> otherwise.</returns>
    /// <param name="id">Identifier.</param>
    public bool IsRobotRight(int id)
    {
        if (dropper.CorrectBot && dropper.CorrectBot.GetInstanceID() == id)
        {
            dropper.CorrectBot = null;
            return true;
        }
        dropper.IncorrectBot = null;
        return false;
    }
    #endregion
}
