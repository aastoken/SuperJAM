using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private
    private int _currentLife = 4;
    // todo (Gabi): This should be START.
    private GameManagerState _currentState = GameManagerState.PLAYING;
    private bool _assignedCoroutineBoxSpawn = false;
    private bool _assignedCoroutineRobotSpawn = false;
    #endregion

    #region Public
    public int lifeStart = 4;
    public int waitSecondsForBoxSpawn = 10;
    public int waitSecondsForRobotSpawn = 10;
    public GameObject boxPrefab = null;
    public GameObject robotPrefab = null;
    public float SceneDimensions = 500.0f;
    public GameObject[] Buttons;
    public readonly Color[] colors = { Color.blue, Color.red, Color.yellow, Color.green };
    #endregion

    #region MonoBehaviour
    void Start()
    {
        _currentLife = lifeStart;
        if (Buttons == null || Buttons.Length <= 0)
        {
            Debug.LogError("ERROR! Set the GameManagers buttons");
        }
    }

    void Update()
    {
        Control();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Control this instance.
    /// </summary>
    void Control()
    {
        switch(_currentState)
        {
            case GameManagerState.START:
                break;
            case GameManagerState.PLAYING:
                if (_currentLife <= 0)
                {
                    _currentState = GameManagerState.FINISH;
                    return;
                }
                StartCoroutinesForSpawn();
                break;
            case GameManagerState.FINISH:

                Debug.Log("Game has finished, todo the end screen");
                break;
            default:
                Debug.LogWarning("State not defined!");
                _currentState = GameManagerState.START;
                break;
        }
    }

    /// <summary>
    /// Instantiates a random box.
    /// </summary>
    IEnumerator InstantiateRandomBox()
    {
            
        while (true)
        {
            Debug.Log("WORK");
            GameObject box = (GameObject)Instantiate(boxPrefab);
            Vector3 randomPosition = ReturnRandomAvailablePosition(100, 100);
            box.transform.position = new Vector3(randomPosition.x, randomPosition.y, randomPosition.y);
            SoundManager.instance.PlayRobotSoundJoint();
            yield return new WaitForSeconds(waitSecondsForBoxSpawn);
        }
    }

    /// <summary>
    /// Instantiates a random robot.
    /// </summary>
    IEnumerator InstantiateRandomRobot()
    {
        while (true)
        {
            SoundManager.instance.PlayRobotSpawn();
            GameObject robot = (GameObject)Instantiate(robotPrefab);
            robot.transform.position = ReturnRandomAvailablePosition(10f, 10f);
            BoxColor r = RandomBoxColor();
            RobotBehaviour rb = robot.GetComponent<RobotBehaviour>();
            if (rb == null)
            {
                Debug.LogError("Error, no robot behaviour available");
            }

            yield return new WaitForSeconds(waitSecondsForRobotSpawn);
        }
    }

    /// <summary>
    /// Returns the random available position depending on the scale of the object.
    /// </summary>
    /// <returns>Vector2: The random available position.</returns>
    public Vector3 ReturnRandomAvailablePosition(float modelLength, float modelWidth)
    {
        Vector3 RandomV = new Vector3(0.0f, 0.0f, 0.0f);
        do
        {
            // Assuming scenario is a square. We should change this
            RandomV = new Vector3(Random.Range(-SceneDimensions, SceneDimensions), 10.0f, Random.Range(-SceneDimensions, SceneDimensions));

        } while (!Physics.CheckBox(RandomV, new Vector3(modelWidth, 0, modelLength)));
        Debug.Log("Random " + RandomV);
        return RandomV;
    }

    /// <summary>
    /// Randoms the color of the box.
    /// </summary>
    /// <returns>The box color.</returns>
    public BoxColor RandomBoxColor()
    {
        int r = Random.Range(0, 4);
        BoxColor c = (BoxColor)r;
        Debug.Log(r + " " + c);
        return c;
    }

    /// <summary>
    /// Starts the coroutines for spawning if they're not set to null.
    /// </summary>
    /// 
    void StartCoroutinesForSpawn()
    {
        if (!_assignedCoroutineBoxSpawn)
        {
            StartCoroutine("InstantiateRandomBox");
            _assignedCoroutineBoxSpawn = true;
        }
        if (!_assignedCoroutineRobotSpawn)
        {
            StartCoroutine("InstantiateRandomRobot");
            _assignedCoroutineRobotSpawn = true;
        }
    }

    /// <summary>
    /// Sets the coroutines to null.
    /// </summary>
    void SetCoroutinesToNull()
    {
        _assignedCoroutineBoxSpawn = false;
        _assignedCoroutineRobotSpawn = false;
    }

    /// <summary>
    /// Finds the button depending on the color you pass.
    /// </summary>
    /// <returns>The button.</returns>
    /// <param name="c">C.</param>
    GameObject FindButton(BoxColor c)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Buttons[i].GetComponent<DoorRobotInteraction>().CurrentColor() == c)
            {
                return Buttons[i];
            }
        }
        Debug.LogError("Color not found for FindButton(), please, check if you passed the right enum.");
        return null;
    }

    /// <summary>
    /// Returns a Button GameObject for Robot Info.
    /// </summary>
    /// <returns>The button to go.</returns>
    /// <param name="c">C.</param>
    public GameObject GiveButton(BoxColor c)
    {
        return FindButton(c);
    }

    /// <summary>
    /// Adds one point to the health.
    /// </summary>
    public void MoreHealth()
    {
        _currentLife++;
    }

    /// <summary>
    /// Lesses the health.
    /// </summary>
    public void LessHealth()
    {
        _currentLife--;
    }
    #endregion 
}
