using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffinityLight : MonoBehaviour
{
    GameManager gm;
    RobotAI IA;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        IA = transform.parent.GetComponent<RobotAI>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        light = transform.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.color = gm.colors[IA.GetGreatestAffinity()];
    }
}
