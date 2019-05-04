using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ajdbjkaf : MonoBehaviour
{

    private float _levitatingSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        //transform.position.y += 0.25 * Mathf.Sin(_levitatingSpeed * dt);
    }
}
