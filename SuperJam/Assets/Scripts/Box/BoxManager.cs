using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    #region Public
    public BoxColor color = BoxColor.GREEN;
    #endregion

    #region Private
    BoxState _currentState = BoxState.NOTPICKED;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        Control();
    }
    #endregion

    #region Methods
    void Control()
    {
        switch (_currentState)
        {
            case BoxState.NOTPICKED:
                break;
            case BoxState.PICKED:
                break;
            default:
                Debug.LogWarning("This state does not exist for BoxManager!");
                break;
        }
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <returns>The state.</returns>
    public BoxState GetState()
    {
        return _currentState;
    }

    #endregion
}
