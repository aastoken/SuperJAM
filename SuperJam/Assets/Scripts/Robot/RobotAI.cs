using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{
    #region Private
    // BLUE, RED, YELLOW, GREEN ...
    private float[] probs = { 1, 1, 1, 1 };
    #endregion 

    public bool Think(GameObject objective)
    {
        BoxManager managerOfTheBoxObjective = objective.GetComponent<BoxManager>();
        if (managerOfTheBoxObjective == null)
        {
            Debug.LogError("DUDE, Whoever is calling this Think() function reconsider your fucking life because you passed a wrong gameobject fuck you");
        }

        float prob = probs[(int)managerOfTheBoxObjective.color] * 100;
        float random = Random.Range(0, 100);

        Debug.Log(random + " " + objective.name);
        return random <= prob;
    }

    /// <summary>
    /// Changes the wanted probability
    /// </summary>
    /// <param name="substractor">Substractor.</param>
    /// <param name="objective" type="BoxColor">Objective.</param>
    public void ChangeProb(float substractor, BoxColor objective)
    {
        probs[(int)objective] -= substractor;
    }
}
