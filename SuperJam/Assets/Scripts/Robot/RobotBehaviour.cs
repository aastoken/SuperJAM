using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    #region Public
    public GameObject testBox = null;
    public BoxColor robotColor = BoxColor.BLUE;
    public GameObject gameManager = null;
    public float aiPercentageDecider = 0.125f;
    public float arenaTamX = 100.0f;
    public float arenaTamZ = 100.0f;
    public Color currentMaterialColor;
    public MeshRenderer modelMaterial;
    #endregion

    #region Private
    public RobotState _currentState = RobotState.SEARCH;
    private BoxRobot _currentBoxTarget = new BoxRobot();
    private BoxRobot _currentBoxPicked = new BoxRobot();
    private RobotMovement _rm;
    private ObjectPicking _op;
    private BoxColor _colorOfRobot;
    private GameManager _gm;
    private RobotAI _ai;
    // Variable that will inform in LeaveBox if the robot is right or not. In the rest of states this will be null, it will be set on the WithBox state.
    private GameObject _door;
    // Wandering
    private bool _finishedMovingWander = true;
    private Vector3 _currentPositionWander = Vector3.one * -1;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        SoundManager.instance.PlayRobotSpawn(gameObject.GetComponent<AudioSource>());
    }

    // Start is called before the first frame update
    void Start()
    {
        HandleErrorsStart();
        SetColor(_gm.RandomBoxColor());
        SetRendererColor();
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    #endregion

    #region Methods
    
    void Control()
    {
        switch (_currentState)
        {
            case RobotState.SEARCH:
                _currentBoxPicked = new BoxRobot();
                _currentBoxTarget = new BoxRobot();
                Wandering();
                break;
            case RobotState.GO:
                HandleGo();
                break;
            case RobotState.TAKEBOX:
                HandleTakeBox();
                SoundManager.instance.PlayRobotSoundMovement(gameObject.GetComponent<AudioSource>()); 
                break;
            case RobotState.WITHBOX:
                // With box function
                // TODO (Gabi) Set the motherfucking Move(Objective);
                HandleWithBox();
                
                break;

            case RobotState.LEAVEBOX:
                SoundManager.instance.PlayRobotPoint(gameObject.GetComponent<AudioSource>());
                Debug.Log("Leave box");
                bool isRobotRight = false;
                isRobotRight = _door.GetComponent<ButtonCommunicator>().Communicate();
                _ai.Learn(aiPercentageDecider, _currentBoxPicked.boxManager.color, isRobotRight);
                HandleIfTheUserIsRight(_currentBoxPicked.boxManager.color, isRobotRight);
                Destroy(_currentBoxPicked.box);
                _currentBoxPicked.boxManager = null;
                _door = null;
                _currentState = RobotState.SEARCH;

                break;
            case RobotState.WAIT:
                break;
        }
    }

    /// <summary>
    /// Wandering this robot instance.
    /// </summary>
    void Wandering()
    {
        // First we generate coordinates for our robot to move towards
        if (_finishedMovingWander)
        {
            Vector2 nextCoords = new Vector2(Random.Range(-arenaTamX, arenaTamX), Random.Range(-arenaTamZ, arenaTamZ));
            _currentPositionWander = new Vector3(nextCoords.x, 1.0f, nextCoords.y);
            _finishedMovingWander = false;
        }
        _rm.Move(_currentPositionWander);
        _finishedMovingWander |= _rm.IsHeNearInstance(_currentPositionWander);
    }

    /// <summary>
    /// Handles the withbox state.
    /// </summary>
    void HandleWithBox()
    {
        // Error handling
        if (_currentBoxPicked.box == null)
        {
            Debug.LogWarning("Current box picked is not found? Probably an error picking it?");
            _currentState = RobotState.SEARCH;
            return;
        }

        if (_currentBoxPicked.box == null)
        {
            Debug.LogWarning("Current box picked script is not found? Probably an error picking it?");
            _currentState = RobotState.SEARCH;
            return;
        }
        // Move to desired door.
        GameObject desiredButton = _gm.GiveButton(_colorOfRobot);
        Vector3 objective = desiredButton.transform.position;
        _rm.Move(objective);
        _op.SetTarget(_currentBoxPicked.box);
        _op.Stay();
        if (_rm.IsHeNearInstance(objective))
        {
            _door = desiredButton;
            _currentState = RobotState.LEAVEBOX;
        }
    }

    /// <summary>
    /// Handles the go state.
    /// </summary>
    void HandleGo()
    {
        // Error handling
        if (_currentBoxTarget.boxManager == null)
        {
            Debug.LogWarning("Current box target is not found? Probably an error finding it?");
            _currentState = RobotState.SEARCH;
            return;
        }
        if (_currentBoxTarget.boxManager.GetState() == BoxState.PICKED)
        {
            _currentBoxTarget = new BoxRobot();
            _currentState = RobotState.SEARCH;
            return;
        }
        // Move to desired box.
        _rm.Move(_currentBoxTarget.box.transform.position);

        if (_rm.IsHeNearInstance(_currentBoxTarget.box.transform.position))
        {
            _currentState = RobotState.TAKEBOX;
            _currentBoxPicked = _currentBoxTarget;
            // Clean target of box.
            _currentBoxTarget = new BoxRobot();

            return;
        }
    }

    /// <summary>
    /// Handles the take box state.
    /// </summary>
    void HandleTakeBox()
    {
        if (_currentBoxPicked.boxManager.GetState() == BoxState.PICKED)
        {
            _currentBoxPicked = new BoxRobot();
            _currentState = RobotState.SEARCH;
            return;
        }
        _op.SetTarget(_currentBoxPicked.box);
        _currentBoxPicked.boxManager.SetPicked();
        _op.PickUpObject();
        _currentState = RobotState.WITHBOX;
        return;
    }

    /// <summary>
    /// Handles the errors start.
    /// </summary>
    void HandleErrorsStart()
    {
        _ai = GetComponent<RobotAI>();

        if (_ai == null)
        {
            Debug.LogError("ERROR! Set the RobotAI Script in the prefab.");
        }
        _rm = GetComponent<RobotMovement>();
        if (_rm == null)
        {
            Debug.LogError("ERROR! Set the RobotMovement Script in the prefab.");
        }
        _op = GetComponent<ObjectPicking>();
        if (_op == null)
        {
            Debug.LogError("ERROR! Set the ObjectPicking Script in the prefab.");
        }
        if (gameManager == null)
        {
            Debug.LogError("ERROR! Set the GameManager object when spawning.");
        }
        _gm = gameManager.GetComponent<GameManager>();
        _colorOfRobot = robotColor;
    }

    void HandleIfTheUserIsRight(BoxColor box, bool right)
    {
        if (right && box != _colorOfRobot)
        {
            _gm.LessHealth();
        }
        if (!right)
        {
            if (right && box == _colorOfRobot) _gm.LessHealth();
        }
    }

    /// <summary>
    /// Sets the state.
    /// </summary>
    /// <param name="state">State.</param>
    public void SetState(RobotState state)
    {
        _currentState = state;
    }

    /// <summary>
    /// Sets the box target.
    /// </summary>
    /// <param name="bt">Bt.</param>
    public void SetBoxTarget(GameObject bt)
    {
        _currentBoxTarget.box = bt;
        _currentBoxTarget.boxManager = bt.GetComponent<BoxManager>();
        Debug.Log("Target set: " + _currentBoxTarget.boxManager);
    }

    /// <summary>
    /// Gets the state of the robot.
    /// </summary>
    /// <returns>The robot state.</returns>
    public RobotState GetRobotState()
    {
        return _currentState;
    }

    /// <summary>
    /// Sets the color.
    /// </summary>
    /// <param name="c">C.</param>
    public void SetColor(BoxColor c)
    {
        _colorOfRobot = c;
    }

    public GameObject GetBoxTarget()
    {
        return _currentBoxTarget.box;
    }

    public void SetRendererColor()
    {
        modelMaterial.materials[0].color = _gm.colors[(int)_colorOfRobot];        
    }


    #endregion
}