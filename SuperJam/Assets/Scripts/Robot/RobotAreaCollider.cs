using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAreaCollider : MonoBehaviour
{
    public GameObject Robot;

    private RobotManager _rm;
    private RobotAI _ra;
    private List<GameObject> _prohibitedBoxes;

    void Start()
    {
        _rm = Robot.GetComponent<RobotManager>();
        _ra = Robot.GetComponent<RobotAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Debug.Log("COLISION");
            if (_ra.Think(other.gameObject))
            {
                _rm.SetBoxTarget(other.gameObject);
                _rm.SetState(RobotState.GO);
            }
            else
            {
                _prohibitedBoxes.Add(other.gameObject);
            }
        }
    }
}
