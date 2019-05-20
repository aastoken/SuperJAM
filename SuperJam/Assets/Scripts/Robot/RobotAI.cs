using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{
    #region Private
    // BLUE, RED, YELLOW, GREEN ...
    public float[] probs = { 1, 1, 1, 1 };
    public float blueAffinity;
    public float yellowAffinity;
    public float redAffinity;
    public float greenAffinity;
    #endregion 

    public void Update()
    {
        blueAffinity = probs[0];
        redAffinity = probs[1];
        yellowAffinity = probs[2];
        greenAffinity = probs[3];
    }

    public float[] getProbs()
    {
        return probs;
    }

    public bool Think(GameObject objective)
    {
        BoxManager managerOfTheBoxObjective = objective.GetComponent<BoxManager>();
        if (managerOfTheBoxObjective == null)
        {
            Debug.LogError("DUDE, Whoever is calling this Think() function reconsider your f***** life because you passed a wrong G.O, **** you");
        }

        float prob = probs[(int)managerOfTheBoxObjective.color] * 100;
        float random = Random.Range(0, 100);

        return random <= prob;
    }

    /// <summary>
    /// Changes the wanted probability
    /// </summary>
    /// <param name="substractor">Substractor.</param>
    /// <param name="objective" type="BoxColor">Objective.</param>
    public void ChangeProb(BoxColor objective, float substractor = 0.0f, float adder = 0.0f)
    {
        probs[(int)objective] -= substractor;
        probs[(int)objective] += adder;
    }

    /// <summary>
    /// Learn with the following information: If the box that he gave is right or not.
    /// </summary>
    /// <param name="decider">How it will change the percentages</param>
    /// <param name="box" type="BoxColor">The box color that he dropped</param>
    /// <param name="right" type="bool">If the decission was right or not</param>
    public void Learn(float decider, BoxColor box, bool right)
    {
        int boxColor = (int)box;
        if (boxColor >= probs.Length) 
            return;

        if (right)
        {
            for (int i = 0; i < probs.Length; i++)
            {
                if (i == boxColor)
                {
                    probs[i] += decider;
                    probs[i] = Mathf.Clamp(probs[i], 0.0f, 1.0f);
                }
                else
                {
                    probs[i] -= decider;
                    probs[i] = Mathf.Clamp(probs[i], 0, 1);
                }
            }
        }     
    }

    public int GetGreatestAffinity()
    {
        float? maxVal = null; //nullable so this works even if you have all super-low negatives
        int index = -1;
        for (int i = 0; i < probs.Length; i++)
        {
            float thisNum = probs[i];
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                index = i;
            }
        }
        return index;
    }

}
