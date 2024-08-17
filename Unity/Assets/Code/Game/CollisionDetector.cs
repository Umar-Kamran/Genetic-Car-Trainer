using System.Linq;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Stores the starting point of the ray
    /// </summary>
    private Vector2[] rayOrigins;

    /// <summary>
    /// Stores the sprite renderer of the object
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Stores the Transform component of the object
    /// </summary>
    private Transform transf;

    /// <summary>
    /// Stores what layers should have collisions detected with
    /// </summary>
    [SerializeField]
    private LayerMask colMask;

    /// <summary>
    /// Height of object
    /// </summary>
    private float Height;

    /// <summary>
    /// Width of object
    /// </summary>
    private float Width;

    /// <summary>
    /// minimum distance of a ray, so nn isn't fed 0
    /// </summary>
    private float minDistance = 0.0325f;

    /// <summary>
    /// Max distance of a ray
    /// </summary>
    private float maxDistance = 5f;

    /// <summary>
    /// starts rays a skinWidth inside the sprite
    /// </summary>
    private const float skinWidth = 0.025f;
    #endregion

    #region Unity Methods
    //Initialises collision detector
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transf = GetComponent<Transform>();
        rayOrigins = new Vector2[5];

        Height = spriteRenderer.bounds.size.y;
        Width = spriteRenderer.bounds.size.x;
    }

    #endregion
    #region Methods
    /// <summary>
    /// sets the origins for 5 rays at the edges of the sprite of the object. each ray is 45 degrees away from each other
    /// </summary>
    /// <remarks>0:left, 1:forward-left, 2: forward, 3:forward-right, 4:right</remarks>
    private void SetRayOrigins()
    {
        rayOrigins[0] = spriteRenderer.transform.TransformPoint(0, Height - skinWidth, 0);
        rayOrigins[1] = spriteRenderer.transform.TransformPoint(Width - skinWidth, Height - skinWidth, 0);
        rayOrigins[2] = spriteRenderer.transform.TransformPoint(Width - skinWidth, 0, 0);
        rayOrigins[3] = spriteRenderer.transform.TransformPoint(Width - skinWidth, -Height + skinWidth, 0);
        rayOrigins[4] = spriteRenderer.transform.TransformPoint(0, -Height + skinWidth, 0);
    }
    /// <summary>
    /// Calculates the distance of the car from any nearby collidable objects
    /// </summary>
    /// <remarks>0:left, 1:forward-left, 2: forward, 3:forward-right, 4:right</remarks>
    /// <returns> A double array of distances</returns>
    /// <remarks>This is done using unitys raycast system</remarks>
    public double[] Distances()
    {
        SetRayOrigins();
        double[] distances = new double[5];

        //directions each ray is travelling in
        Vector2[] directions =
            {
            transf.up,
            (transf.up + transf.right).normalized,
            transf.right,
            (transf.right - transf.up).normalized,
            -transf.up,
            };

        for (int i = 0; i < 5; i++)
        {
            Vector2 curRayOrigin = rayOrigins[i];
            RaycastHit2D hit = Physics2D.Raycast(curRayOrigin, directions[i], maxDistance, colMask);

            if (hit)
                distances[i] = hit.distance;
            else
                distances[i] = maxDistance;

            if (distances[i] < minDistance)
                distances[i] = 0;
            Debug.DrawRay(curRayOrigin, directions[i] * maxDistance, Color.red, Time.deltaTime);
        }
        return distances;
    }
    /// <summary>
    /// Checks for collisions and translates and rotates the car. Returns true if it crashes, false otherwise
    /// </summary>
    /// <param name="Velocity">Distance to translate by</param>
    /// <param name="turn">angle to rotate by</param>
    /// <returns>True if car has crashed</returns>
    public bool Move(float Velocity, float turn)
    {
        bool Crashed = false;
        double[] distances = Distances();
        if (distances.Contains(0))
            Crashed = true;
        else
        {
            if (distances[2] < Velocity)
            {
                Crashed = true;
                Velocity = (float)distances[2];
            }
            transf.Translate(new Vector3(Velocity, 0, 0));
            transf.Rotate(new Vector3(0, 0, turn));
        }

        return Crashed;
    }
    #endregion
}
