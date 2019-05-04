using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAreaCollider : MonoBehaviour
{
    #region Public
    public GameObject Robot;
    #endregion

    #region Private 
    private RobotManager _rm;
    private RobotAI _ra;
    private List<int> _prohibitedBoxes = new List<int>();
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _rm = Robot.GetComponent<RobotManager>();
        _ra = Robot.GetComponent<RobotAI>();
    }


    /// <summary>
    /// Trigger Enter
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if (!_prohibitedBoxes.Contains(other.gameObject.GetInstanceID()) && _ra.Think(other.gameObject))
            {
                _rm.SetBoxTarget(other.gameObject);
                _rm.SetState(RobotState.GO);
            }
            else
            {
                _prohibitedBoxes.Add(other.gameObject.GetInstanceID());
            }
        }
    }
    #endregion
}
