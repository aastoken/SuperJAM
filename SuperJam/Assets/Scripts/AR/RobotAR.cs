using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class RobotAR : MonoBehaviour
{
    #region Public
    public AxisMobile axis;
    public int score;
    public GameObject redBox;
    public Transform ar;
    public DefaultTrackableEventHandler df;
    public float speed = 10f;
    #endregion

    #region Private
    private Coroutine current;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        float axisHorizontal = axis.GetAxis("Horizontal");
        float axisForward = axis.GetAxis("Forward");
        Debug.Log(df.GetComponent<TrackableBehaviour>().CurrentStatus);
        if (df.GetComponent<TrackableBehaviour>().CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            if (current == null) current = StartCoroutine(SpawnBoxes());
            GetComponent<Rigidbody>().velocity = Vector3.right * axisHorizontal * speed * Time.deltaTime + axisForward * speed * Vector3.forward * Time.deltaTime;
            if (Mathf.Abs(axisHorizontal) > 0.01f || Mathf.Abs(axisForward) > 0.01f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(axisHorizontal, axisForward) * Mathf.Rad2Deg, transform.eulerAngles.z);
            }
        }
    }
    #endregion

    #region Methods
    private IEnumerator SpawnBoxes()
    {
        yield return new WaitForSeconds(2f);
        GameObject rb = Instantiate(redBox, ar);
        redBox.transform.localPosition = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        current = null;
    }
    #endregion
}
