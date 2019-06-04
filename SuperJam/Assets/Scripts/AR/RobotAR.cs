using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAR : MonoBehaviour
{
    public AxisMobile axis;
    public int score;
    public GameObject redBox;
    public Transform ar;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(SpawnBoxes());
    }

    void Update()
    {
        float axisHorizontal = axis.GetAxis("Horizontal");
        float axisForward = axis.GetAxis("Forward");

        Debug.Log(axisHorizontal);
        transform.position += Vector3.right * axisHorizontal * 10 * Time.deltaTime + axisForward * Vector3.forward * 10 * Time.deltaTime;
        transform.LookAt(Vector3.forward, Vector3.up);
    }

    IEnumerator SpawnBoxes()
    {
        yield return new WaitForSeconds(2f);
        GameObject rb = Instantiate(redBox, ar);
        redBox.transform.localPosition = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        StartCoroutine(SpawnBoxes());
    }
}
