using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCountToDeath : MonoBehaviour
{
    #region Variables
    public int countLeft = 5;
    private int _countLeft = 5;
    #endregion

    #region Method
    public void SubstractOne()
    {
        _countLeft -= 1;
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _countLeft = countLeft;
    }

    void Update()
    {
        if (_countLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
