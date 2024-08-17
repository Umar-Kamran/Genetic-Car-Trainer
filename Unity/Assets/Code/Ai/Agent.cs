using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Car script of this agent
    /// </summary>
    private Car car;

    /// <summary>
    /// Neural network for this agent
    /// </summary>
    public NeuralNetwork neuralNetwork { get; private set; }

    /// <summary>
    /// Genome of this agent
    /// </summary>
    private Genome genome;


    /// <summary>
    /// If agent has crashed, this is true, stopping the agents functionality
    /// </summary>
    public bool crashed { get; private set; }

    /// <summary>
    /// stores the collision detector for this car
    /// </summary>
    private CollisionDetector colDetector;

    /// <summary>
    /// Stores a list of checkpoints/scores this agent has collided with, prevents scores from being counted twice
    /// </summary>
    private List<GameObject> pointsCollided;


    /// <summary>
    /// Stores if agent has reached the end
    /// </summary>
    public bool Finished { get; private set; }
    #endregion


    #region Constructor
    /// <summary>
    /// Acts in a similar way as a constructor. Initialises all values of this agent. Requires a genome
    /// </summary>
    /// <param name="car">Car script for this agent</param>
    /// <param name="genome">Genome for this agent</param>
    /// <param name="LayerShaping">Topology of neural network</param>
    /// <param name="colDetector">Collision detector for the car of this agent</param>
    public void Init(Car car, Genome genome, uint[] LayerShaping, CollisionDetector colDetector)
    {
        Finished = false;
        crashed = false;
        this.car = car;
        neuralNetwork = new NeuralNetwork(LayerShaping);
        this.genome = genome;
        neuralNetwork.SetParameters(genome.Genes);
        this.colDetector = colDetector;
        pointsCollided = new List<GameObject>();
    }

    /// <summary>
    /// Acts in a similar way as a constructor. Initialises all values of this agent. Creates a genome
    /// </summary>
    /// <param name="car">Car script for this agent</param>
    /// <param name="LayerShaping">Topology of neural network</param>
    /// <param name="colDetector">Collision detector for the car of this agent</param>
    public void Init(Car car, uint[] LayerShaping, CollisionDetector colDetector)
    {
        Finished = false;
        pointsCollided = new List<GameObject>();
        crashed = false;
        this.car = car;
        neuralNetwork = new NeuralNetwork(LayerShaping);
        neuralNetwork.ResetLayers(1, -0.5);
        this.colDetector = colDetector;
    }
    #endregion


    #region Unity Methods

    // Update is called once per frame
    void FixedUpdate()
    {
        //handles the feedforward and movement of the car
        if (crashed)
            return;
        double[] distance = colDetector.Distances();

        if (distance.Length != neuralNetwork.LayerShaping[0])
            throw new System.Exception("neural shaping is incorrect");

        double[] output = neuralNetwork.FeedForward(distance);

        if (output.Length != 2)
            throw new System.Exception("neural shaping is incorrect");

        car.DeltaSpeed(output[0]);
        car.DeltaTurn(output[1]);
        crashed = colDetector.Move(car.Velocity, car.TurnDelta);
    }
    #endregion



    #region Methods
    /// <summary>
    /// Set genome of agent
    /// </summary>
    /// <param name="genome">Genome to change to</param>
    public void SetGenome(Genome genome)
    {
        this.genome = genome;
    }

    //Handles collisions with checkpoints, to increase fitness of genome
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "point" & !pointsCollided.Contains(obj))
        {
            pointsCollided.Add(obj);
            genome.Fitness += obj.GetComponent<Score>().GetPoints();
        }
        else if (obj.tag == "end")
        {
            Finished = true;
            crashed = true;
        }
    }
    #endregion
}
