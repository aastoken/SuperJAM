using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFillLevels : MonoBehaviour
{
    RobotAI rabo;

    

    float maxAffinity = 0.8f;

    private Material blueFill;
    private Material yellowFill;
    private Material redFill;
    private Material greenFill;

    // Start is called before the first frame update
    void Start()
    {
        rabo = transform.parent.parent.GetComponent<RobotAI>();
        Material[] lol = GetComponent<MeshRenderer>().materials;
        blueFill = lol[0];
        yellowFill = lol[3];
        redFill = lol[1];
        greenFill = lol[2];

    }

    // Update is called once per frame
    void Update()
    {
        ManageFillLevel();
    }

    void ManageFillLevel()
    {
        float maxL = -1.32f;
        float minL = -1.151f;
        float delta = minL - maxL;
        float tempB = getTemp(rabo.blueAffinity, maxL, delta);
        float tempY = getTemp(rabo.yellowAffinity, maxL, delta);
        float tempR = getTemp(rabo.redAffinity, maxL, delta);
        float tempG = getTemp(rabo.greenAffinity, maxL, delta);
        blueFill.SetFloat("_FillAmount", Mathf.Lerp(minL, maxL, (tempB - maxL) / (minL - maxL)));//inverse proportion
        yellowFill.SetFloat("_FillAmount", Mathf.Lerp(minL, maxL, (tempY - maxL) / (minL - maxL)));//inverse proportion
        redFill.SetFloat("_FillAmount", Mathf.Lerp(minL, maxL, (tempR - maxL) / (minL - maxL)));//inverse proportion
        greenFill.SetFloat("_FillAmount", Mathf.Lerp(minL, maxL, (tempG - maxL) / (minL - maxL)));//inverse proportion

    }

    float getTemp(float baseValue, float maxL, float delta)
    {
        return (delta / 100) * (baseValue / maxAffinity * 100) + maxL;//direct proportion
    }
}
