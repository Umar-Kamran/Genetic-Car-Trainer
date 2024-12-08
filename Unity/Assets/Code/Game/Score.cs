using UnityEngine;

public class Score : MonoBehaviour
{
    #region Attributes

    /// <summary>
    /// an array of points, 1st car gets index 0, 2nd index 1. minimum points is last index
    /// </summary>
    [SerializeField]
    private float[] points =
    {
        1.5f,
        1.2f,
        1.0f,
    };

    /// <summary>
    /// Stores how many cars have crossed this collider
    /// </summary>
    private int CarsCrossedPoint;

    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        CarsCrossedPoint = 0;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the points obtained by crossing this checkpoint
    /// </summary>
    /// <returns></returns>
    public float GetPoints()
    {

        CarsCrossedPoint++;
        return points[Mathf.Clamp(CarsCrossedPoint - 1, 0, points.Length - 1)];
    }

    /// <summary>
    /// Resets checkpoint
    /// </summary>
    public void Reset()
    {
        CarsCrossedPoint = 0;
    }
    #endregion
}
