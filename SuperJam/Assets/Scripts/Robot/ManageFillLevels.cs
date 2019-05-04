using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFillLevels : MonoBehaviour
{
    RobotAI rabo;

    public float blueAffinity;
    public float yellowAffinity;
    public float redAffinity;
    public float greenAffinity;

    float maxAffinity = 80f;

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
        blueAffinity = rabo.getProbs()[0];
        redAffinity = rabo.getProbs()[1];
        yellowAffinity = rabo.getProbs()[2];
        greenAffinity = rabo.getProbs()[3];
    }

    void ManageFillLevel()
    {
        float maxL = 0.5f;
        float minL = 1.36f;
        float delta = minL - maxL;
        float tempB = getTemp(blueAffinity, maxL, delta);
        float tempY = getTemp(yellowAffinity, maxL, delta);
        float tempR = getTemp(redAffinity, maxL, delta);
        float tempG = getTemp(greenAffinity, maxL, delta);
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
