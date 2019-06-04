using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public RobotAR robot;
    public TMPro.TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + robot.score.ToString();
    }
}
