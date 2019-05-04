using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAreaCollider : MonoBehaviour
{
    #region Public
    public GameObject Robot;
    #endregion

    #region Private 
    private RobotBehaviour _rm;
    private RobotAI _ra;
    private List<int> _prohibitedBoxes = new List<int>();
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        _rm = Robot.GetComponent<RobotBehaviour>();
        _ra = Robot.GetComponent<RobotAI>();
    }


    /// <summary>
    /// Trigger Enter
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box") && _rm.GetRobotState() == RobotState.SEARCH)
        {
            ;
            BoxManager otherManager = other.GetComponent<BoxManager>();

            if (otherManager.GetState() == BoxState.PICKED)
            {
                return;
            }

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

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Box") && _rm != null && other.gameObject != null && _rm.GetBoxTarget().GetInstanceID() == other.gameObject.GetInstanceID())
            //_rm.SetState(RobotState.SEARCH);
    }
    #endregion
}
