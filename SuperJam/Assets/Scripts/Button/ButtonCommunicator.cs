using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCommunicator : MonoBehaviour
{
    #region Public

    #endregion

    #region Private
    ButtonManager _bm;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _bm = GetComponent<ButtonManager>();
        if (_bm == null)
        {
            Debug.LogError("Error, not finding button manager");
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Communicate from this instance to the robot calling it if the decission was right.
    /// </summary>
    /// <returns>The communicate.</returns>
    public bool Communicate()
    {
        return _bm.DecissionRightOrNot();
    }
    #endregion
}
