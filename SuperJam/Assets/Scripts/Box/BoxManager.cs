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
    GameManager _gm; 
    #endregion

    #region MonoBehaviour
    void Update()
    {
        Control();
    }

    void Start()
    {
        GameObject g = GameObject.FindWithTag("GameManager");
        _gm = g.GetComponent<GameManager>();
        SetColor(_gm.RandomBoxColor());
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

    public void SetPicked()
    {
        _currentState = BoxState.PICKED;
    }

    public void SetColor(BoxColor c)
    {
        color = c;
        GetComponent<MeshRenderer>().materials[0].color = _gm.colors[(int)c];
    }
    #endregion
}
