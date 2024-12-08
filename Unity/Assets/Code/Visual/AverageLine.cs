using System.Collections.Generic;
using UnityEngine;

public class AverageLine : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Stores the average location of running cars per tick
    /// </summary>
    private List<Vector3> AveragePositions;

    /// <summary>
    /// Stores a reference to the lineRenderer component. This is used to display the line
    /// </summary>
    [SerializeField]
    private LineRenderer lineRenderer;
    #endregion

    #region Methods

    /// <summary>
    /// Resets the lineRenderer by removing all points and resets the list storing positions
    /// </summary>
    public void Reset()
    {

        lineRenderer.positionCount = 0;
        AveragePositions = new List<Vector3>();
    }

    /// <summary>
    /// Adds the average location of a list of positions to the line
    /// </summary>
    /// <param name="positions">array of vector3 of positions to be averaged</param>
    public void AddPositions(Vector3[] positions)
    {
        Vector3 average = Vector3.zero;
        foreach (var position in positions)
            average += position;
        average /= positions.Length;
        AveragePositions.Add(average);
    }

    /// <summary>
    /// Adds to vector 3 position to the line renderer
    /// </summary>
    /// <param name="position">Position of gameObject</param>
    public void AddPositions(Vector3 position)
    {
        AveragePositions.Add(position);

    }

    /// <summary>
    /// Provides line renderer with necessary information to display the positions
    /// </summary>
    public void DisplayLine()
    {
        lineRenderer.positionCount = AveragePositions.Count;
        lineRenderer.SetPositions(AveragePositions.ToArray());
        lineRenderer.Simplify(0.05f);
    }
    #endregion
}
