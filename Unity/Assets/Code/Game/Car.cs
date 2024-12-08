using UnityEngine;

public class Car : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// The velocity of the car
    /// </summary>
    public float Velocity { get; private set; }

    /// <summary>
    /// Max acceleration per game tick
    /// </summary>
    private float MaxAcceleration;

    /// <summary>
    /// Maximum amount car can turn per tick
    /// </summary>
    private float MaxTurnAngle;

    /// <summary>
    /// Max speed the car can reach
    /// </summary>
    [SerializeField]
    private float MaxSpeed;

    /// <summary>
    /// turning angle
    /// </summary>
    public float TurnDelta { get; private set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Creates an instance of Car setting initial values
    /// </summary>
    /// <param name="MaxTurnAngle">Max angle car can turn in one game tick</param>
    /// <param name="MaxSpeed">Max speed car can reach</param>
    /// <param name="MaxAcceleration">Max acceleration per game tick</param>
    public void ResetCar(float MaxTurnAngle, float MaxSpeed, float MaxAcceleration)
    {
        Velocity = 0f;
        TurnDelta = 0f;
        this.MaxTurnAngle = MaxTurnAngle;
        this.MaxSpeed = MaxSpeed;
        this.MaxAcceleration = MaxAcceleration;
    }


    #endregion

    #region Methods


    /// <summary>
    /// Accelerates/decelerates the car with the change being proportional to the multiplier, clamps velocity between 0 and the max speed
    /// </summary>
    /// <param name="Multiplier"> value between -1 to 1 to change speed by</param>
    /// <exception cref="System.Exception">acceleration multiplier must be -1 <= multiplier <= 1</exception>
    public void DeltaSpeed(double Multiplier)
    {
        if (Multiplier < -1.0f || Multiplier > 1.0f)
            throw new System.Exception("Acceleration multiplier must be -1 <= multiplier <= 1");
        Velocity += (float)Multiplier * MaxAcceleration;

        Velocity = Mathf.Clamp(Velocity, 0.1f, MaxSpeed);
    }


    /// <summary>
    /// sets the turn angle to a value proportional to the multiplier
    /// </summary>
    /// <param name="Multiplier">value between -1 to 1 to turn by</param>
    /// <exception cref="System.Exception"> Turn multiplier must be -1 <= multiplier >= 1</exception>
    public void DeltaTurn(double Multiplier)
    {
        if (Multiplier < -1.0f || Multiplier > 1.0f)
            throw new System.Exception("Turn multiplier must be -1 <= multiplier >= 1");

        TurnDelta = (float)Multiplier * MaxTurnAngle;
    }


    #endregion
}
