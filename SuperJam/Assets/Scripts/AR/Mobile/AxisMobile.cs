using UnityEngine;

public enum ButtonState
{
    None = 0,
    LeftL = -1,
    LeftR = -1,
    RightL = 1,
    RightR = 1,
    UpL = 1,
    Up = 1,
    DownL = -1,
    DownR = -1,
}


/// <summary>
/// Mini Component Library that you can modify and add new axis so 
/// you don't have to call multiple functions to make sure that you get
/// an axis response. Here you will be able to get from a single function
/// an axis user response, doesn't matter if you're on mobile or pc.
/// </summary>
public class AxisMobile : MonoBehaviour
{
    #region Public
    public ButtonState currentStateHorizontal = ButtonState.None;
    public ButtonState currentStateVertical = ButtonState.None;
    public ButtonState currentStateVerticalRotation = ButtonState.None;
    public ButtonState currentStateHorizontalRotation = ButtonState.None;
    // here add more states that you can set from buttons (if necessary).
    #endregion

    #region MonoBehaviour
    void Start()
    {
        // If is mobile, activate.
        Input.gyro.enabled |= IsMobile();
    }
    #endregion

    #region Methods
    public Vector3 GyroscopeEuler()
    {
        Quaternion gyroCurrentState = Input.gyro.attitude;
        Vector3 gyroEulerAngles = gyroCurrentState.eulerAngles;
        return gyroEulerAngles;
    }

    public Quaternion Gyroscope()
    {
        return Input.gyro.attitude;
    }

    /// <summary>
    /// Returns <paramref name="axis"/> from gamepad, keyboard or mobile button configured in the project, pass the boolean <paramref name="IGNORE_IS_MOBILE"/> = true
    /// that ignores the IsMobile() function in case you see that Application.IsMobilePlatform bugs and you only want GamePad and Keyboard Inputs.
    /// </summary>
    /// <returns>The axis.</returns>
    /// <param name="axis">Axis.</param>
    /// <param name="IGNORE_IS_MOBILE">Boolean that makes sure that you always get a Keyboard or GamePad Input</param>
    public float GetAxis(string axis, bool IGNORE_IS_MOBILE = false)
    {
        if (IsMobile() || IGNORE_IS_MOBILE)
            switch (axis)
            {
                case "Horizontal":
                    return Input.GetAxis("Horizontal");
                case "Vertical":
                    return Input.GetAxis("Vertical");
                case "HorizontalRotation":
                    return Input.GetAxis("HorizontalRotation");
                case "VerticalRotation":
                    return Input.GetAxis("VerticalRotation");            
                case "Find":
                    return Input.GetAxis("Find");
                case "Jump":
                    return Input.GetAxis("Jump");
                default:
                    return Input.GetAxis(axis);
                    return 0;
            }
        return GetAxisMobile(axis);
    }

    /// <summary>
    /// Returns a mobile axis, take in mind that if you're not on mobile this function will work anyways but
    /// for better practice instead use GetAxis();
    /// </summary>
    /// <returns>The axis mobile.</returns>
    /// <param name="axis">Axis.</param>
    public float GetAxisMobile(string axis)
    {
        if (!IsMobile())
            switch (axis)
            {
                // Horizontal movement.
                case "Horizontal":
                    return (int)currentStateHorizontal;
                // Forward movement
                case "Forward":
                    return (int)currentStateVertical;
                // Rotation
                case "HorizontalRotation":
                    return (int)currentStateHorizontalRotation;
                // Go vertical
                case "Vertical":
                    return (int)currentStateVerticalRotation;
                // Rotation
                case "Rotation":
                    return (int)currentStateHorizontalRotation;
                case "Find":
                    Debug.LogWarning("Be careful, you shouldn't be calling this case of GetAxisMobile if you can be on PC platform!");
                    return 0;
                case "Jump":
                    Debug.LogWarning("Be careful, you shouldn't be calling this case of GetAxisMobile if you can be on PC platform!");
                    return 0;
                default:
                    Debug.LogWarning("No axis assigned.");
                    return 0;
            }

        return GetAxis(axis, true);

    }

    public float GetAxisGyroscope(string axis)
    {
        if (IsMobile())
            switch (axis)
            {
                case "Horizontal":
                    return GyroscopeEuler().x;
                case "Vertical":
                    return GyroscopeEuler().z;
                default:
                    return 0.0f;
            }
        return 0.0f;
    }

    /// <summary>
    /// Sets the state of the axis movement related.
    /// </summary>
    /// <param name="state">State.</param>
    /// <param name="who">Who.</param>
    public void SetState(ButtonState state, string who)
    {
        switch (who)
        {
            case "Horizontal":
                currentStateHorizontal = state;
                break;
            case "HorizontalRotation":
                currentStateHorizontalRotation = state;
                break;
            case "VerticalRotation":
                currentStateVerticalRotation = state;
                break;
            case "Rotation":
                currentStateHorizontalRotation = state;
                break;
            case "Forward":
                currentStateVertical = state;
                break;
            case "Vertical":
                currentStateVerticalRotation = state;
                break;       
            default:
                currentStateVertical = state;
                break;
        }
    }

    /// <summary>
    /// Returns true if we are on a mobile platform (clearer way)
    /// </summary>
    /// <returns><c>true</c>, if mobile <c>false</c> otherwise.</returns>
    public bool IsMobile()
    {
        return Application.isMobilePlatform;
    }

    #endregion
}
