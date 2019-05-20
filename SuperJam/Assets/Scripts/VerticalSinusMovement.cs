using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSinusMovement : MonoBehaviour {

    public float period = 1;
    public float magnitude = 2;
    public float delay = 0;
    public float speedRotation = 5.0f;
    public bool random = true;
    public bool rotate = false;
    float realTime = 0;
    public bool destroyOnCollision = true;
    
	// Use this for initialization
	void Start () {
        if (random) delay = Random.Range(0, 2);
        realTime = delay;
        

    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        Sinus(dt);

        if (rotate == true){
            transform.Rotate(0.0f, 0.0f, speedRotation * dt);
        }
	}

    void Sinus(float dt)
    {
        realTime += dt;
        realTime %= period;

        float sinus = Mathf.Sin(Mathf.Abs(realTime * 2 * Mathf.PI / period)) * magnitude;
        transform.Translate(Vector3.up * sinus, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&& destroyOnCollision)
        { 
            Destroy(gameObject);
        }
    }
}
