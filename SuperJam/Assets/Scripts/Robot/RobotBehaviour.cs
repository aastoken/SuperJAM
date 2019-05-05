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
    public MeshRenderer modelLeftArm;
    public MeshRenderer modelRightArm;
    public int adderForHighScore = 100;
    #endregion

    #region Private
    public RobotState _currentState = RobotState.SEARCH;
    private GameObject _desiredDropPoint = null;
    private BoxRobot _currentBoxTarget = new BoxRobot();
    private BoxRobot _currentBoxPicked = new BoxRobot();
    private RobotMovement _rm;
    private ObjectPicking _op;
    private GameManager _gm;
    private RobotAI _ai;
    private RobotCountToDeath _rcd;
    private BoxColor _colorOfRobot;
    // Variable that will inform in LeaveBox if the robot is right or not. In the rest of states this will be null, it will be set on the WithBox state.
    private GameObject _door;
    // Wandering
    private bool _finishedMovingWander = true;
    private Vector3 _currentPositionWander = Vector3.one * -1;
    bool droppedInRight = false;
    #endregion

    #region MonoBehaviour

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        SoundManager.instance.PlayRobotSpawn(gameObject.GetComponent<AudioSource>());
    }

    void Start()
    {
        HandleErrorsStart();
        SetColor(_gm.RandomBoxColor());
        SetRendererColor();
    }

    void Update()
    {
        Control();
    }

    #endregion

    #region Methods
    
    void Control()
    {
        Debug.Log(_currentState);
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
                HandleWithBox();
                break;            
            case RobotState.QUEUE:
                HandleQueue();
                break;
            case RobotState.GOTOWAITZONE:
                HandleGoToWaitZone();
                break;
            case RobotState.INWAITZONE:
                HandleInWaitZone();
                break;
            case RobotState.GONNADROP:
                HandleGonnaDrop();
                break;
            
            case RobotState.LEAVEBOX:
                HandleLeaveBox();
                break;
            case RobotState.EXIT:
                HandleExitDropper(droppedInRight);
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
            Vector3 nextCoords = _gm.SpawnFromTheCenter();
            _currentPositionWander = new Vector3(nextCoords.x, transform.position.y, nextCoords.z);
            _finishedMovingWander = false;
        }
        _rm.Move(_currentPositionWander);
        _finishedMovingWander |= _rm.IsHeNearInstance(_currentPositionWander);
    }

    #region Handlers

    void HandleExitDropper(bool isRight)
    {
        DropPointLogic dropper = _door.GetComponent<DoorRobotInteraction>().dropper;
        PointsDrop use = PointsDrop.EXITCORRECT;
        if (isRight)
        {
            use = PointsDrop.EXITCORRECT;
            _rm.Move(dropper.GetPoint(use));
        }
        else
        {
            use = PointsDrop.EXITINCORRECT;
            _rm.Move(dropper.GetPoint(use));
        }



        if (_rm.IsHeNearInstance(dropper.GetPoint(use)))
        {
            _door = null;
            _currentState = RobotState.SEARCH;
        }
    }
    void HandleLeaveBox()
    {
        SoundManager.instance.PlayRobotPoint(gameObject.GetComponent<AudioSource>());
        DoorRobotInteraction DRI = _door.GetComponent<DoorRobotInteraction>();
        Debug.Log("Leave box");
        bool isRobotRight = false;
        isRobotRight = DRI.IsRobotRight(GetInstanceID());
        _ai.Learn(aiPercentageDecider, _currentBoxPicked.boxManager.color, isRobotRight);
        HandleIfTheUserIsRight(_currentBoxPicked.boxManager.color, isRobotRight);
        Destroy(_currentBoxPicked.box);
        AddScore(_currentBoxPicked.boxManager.color, _colorOfRobot, isRobotRight);
        _currentBoxPicked.boxManager = null;
        _currentState = RobotState.EXIT;
        

    }

    bool HandleGonnaDrop()
    {
        DropPointLogic dropper = _door.GetComponent<DoorRobotInteraction>().dropper;
        PointsDrop can_ = dropper.WhatDrop(gameObject.GetInstanceID());
        Vector3 pt = dropper.GetPoint(can_ == PointsDrop.DROPRIGHT ? PointsDrop.VENTCORRECT : PointsDrop.VENTINCORRECT);
        transform.LookAt(pt);
        //if (dropper.WhereIsHe())
        //{
        //    what = true; //if he's going to the correct path, go true
        //}
        //else what = false; //if not, go false.
        droppedInRight = dropper.WhereIsHe();
        if (_rm.IsHeLookingAt(pt))
        {
            _currentState = RobotState.LEAVEBOX;
            Debug.Log("Yo i'm lookin at the droppah");
            return dropper.WhereIsHe();
        }
        return dropper.WhereIsHe();
    }

    void HandleInWaitZone()
    {
        DropPointLogic dropper_ = _door.GetComponent<DoorRobotInteraction>().dropper;
        PointsDrop can = dropper_.WhatDrop(gameObject.GetInstanceID());
        _op.SetTarget(_currentBoxPicked.box);
        _op.Stay();
        dropper_.waitZone = this;
        if (can != PointsDrop.NOTHING)
        {
            _rm.Move(dropper_.GetPoint(can));
            if (_rm.IsHeNearInstance(dropper_.GetPoint(can)))
            {
                dropper_.SetIDLE();
                dropper_.SetBotInDropZone(this, can);
                Debug.Log("WE ARE GODS");
                _currentState = RobotState.GONNADROP;
                dropper_.waitZone = null;
            }
        }
    }

    void HandleGoToWaitZone()
    {
        DropPointLogic dropperForWaitZone = _door.GetComponent<DoorRobotInteraction>().dropper;
        _rm.Move(dropperForWaitZone.GetPoint(PointsDrop.WAITSTATION));
        _op.SetTarget(_currentBoxPicked.box);
        _op.Stay();
        if (_rm.IsHeNearInstance(dropperForWaitZone.GetPoint(PointsDrop.WAITSTATION)))
        {
            _currentState = RobotState.INWAITZONE;
            _rm.SetInstantSpeed(0);
        }
    }



    void HandleQueue()
    {
        DropPointLogic dropper = _door.GetComponent<DoorRobotInteraction>().dropper;
        _op.SetTarget(_currentBoxPicked.box);
        _op.Stay();
        if (dropper.CanIGo(gameObject.GetInstanceID()))
        {
            _currentState = RobotState.GOTOWAITZONE;
            _rm.SetInstantSpeed(0);
        }
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
        _desiredDropPoint = _gm.GiveDoor(_colorOfRobot);
        Vector3 objective = _desiredDropPoint.transform.position;
        _rm.Move(objective);
        _op.SetTarget(_currentBoxPicked.box);
        _op.Stay();
        if (_rm.IsHeNearInstance(objective))
        {
            _door = _desiredDropPoint;
            DropPointLogic dropper = _door.GetComponent<DoorRobotInteraction>().dropper;
            dropper.AddToQueue(this);
            Debug.LogWarning("DOOR WARNING  " + _door.GetInstanceID());
            _currentState = RobotState.QUEUE;
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
        _rcd = GetComponent<RobotCountToDeath>();

        if (_rcd == null)
        {
            Debug.LogError("ERROR! Set the RobotCountToDeath Script in the prefab.");
        }
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
        if (right && box == _colorOfRobot)
        {
            _rcd.SubstractOne();
        }
    }

    #endregion

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

    /// <summary>
    /// Gets the box target.
    /// </summary>
    /// <returns>The box target.</returns>
    public GameObject GetBoxTarget()
    {
        return _currentBoxTarget.box;
    }

    /// <summary>
    /// Sets the color of the renderer.
    /// </summary>
    public void SetRendererColor()
    {
        modelMaterial.materials[0].color = _gm.colors[(int)_colorOfRobot];
        modelLeftArm.materials[0].color = _gm.colors[(int)_colorOfRobot];
        modelRightArm.materials[0].color = _gm.colors[(int)_colorOfRobot];
    }

    void AddScore(BoxColor box, BoxColor robotColor, bool userDecission)
    {
        if (userDecission && box == robotColor)
        {
            _gm.score += 100;
        }
    }

    #endregion
}