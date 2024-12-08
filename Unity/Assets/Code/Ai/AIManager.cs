using Cinemachine;
using TMPro;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// stores the prefab of the car to allow instantiation
    /// </summary>
    [SerializeField]
    private GameObject CarPrefab;

    /// <summary>
    /// initial car maxSpeed
    /// </summary>
    [SerializeField]
    private float maxSpeed;

    /// <summary>
    /// initial car maxTurnAngle
    /// </summary>
    [SerializeField]
    private float maxTurnAngle;

    /// <summary>
    /// max acceleration of car
    /// </summary>
    [SerializeField]
    private float maxAcceleration;

    /// <summary>
    /// number of agents
    /// </summary>
    [SerializeField]
    private int carCount = 200;

    /// <summary>
    /// Stores an array of all cars game objects
    /// </summary>
    private GameObject[] carObjects;

    /// <summary>
    /// Stores an array of all cars "Car.cs" scripts
    /// </summary>
    private Car[] cars;

    /// <summary>
    /// Stores an array of all agents
    /// </summary>
    private Agent[] agents;

    /// <summary>
    /// start of track
    /// </summary>
    [SerializeField]
    private Vector2 startingPosition = Vector2.zero;

    /// <summary>
    /// direction facing at start
    /// </summary>
    [SerializeField]
    private Vector3 startingRotation = Vector3.zero;

    /// <summary>
    /// stores the genomes of this population
    /// </summary>
    private Genome[] populationGenomes;

    /// <summary>
    /// Topology of neural network for cars
    /// </summary>
    [SerializeField]
    private uint[] layerShaping = { 5, 2 };

    /// <summary>
    /// Array containing a reference to each checkpoint score script
    /// </summary>
    [SerializeField]
    private Score[] CheckpointScores;

    /// <summary>
    /// stores a reference to the AverageLine script 
    /// </summary>
    [SerializeField]
    private AverageLine RacingLine;

    /// <summary>
    /// If true, evolution occurs even if not all cars have crashed
    /// </summary>
    [SerializeField]
    private bool ForceEvolve;

    /// <summary>
    /// stores a reference to the SpriteChanger script 
    /// </summary>
    [SerializeField]
    private SpriteChanger spriteChanger;

    /// <summary>
    /// Stores a reference to the camera script
    /// </summary>
    [SerializeField]
    private MoveCamera cam;

    /// <summary>
    /// What number generation is this
    /// </summary>
    private int generationCount;

    /// <summary>
    /// Time this generation has been alive
    /// </summary>
    private float LapTime;

    /// <summary>
    /// Time last generation first finished
    /// </summary>
    private float LastLapTime;

    /// <summary>
    /// text object for generation
    /// </summary>
    [SerializeField]
    private TMP_Text GenerationText;

    /// <summary>
    /// text object for lap time
    /// </summary>
    [SerializeField]
    private TMP_Text LapTimeText;

    /// <summary>
    /// text for last lap time
    /// </summary>
    [SerializeField]
    private TMP_Text LastLapTimeText;

    #endregion

    #region Unity Methods

    // Update is called once per frame
    void FixedUpdate()
    {
        DisplayLine();
        //EvaluateFitness();
        SpriteChanging();
        Evolve();
        cam.ChangeCar(GetFittestCar());
    }
    private void Update()
    {
        if (!CarFinished())
        {
            LapTime += Time.deltaTime;
            SetText();
        }
    }
    #endregion

    #region Methods

    public void Init()
    {
        generationCount = 1;
        LapTime = 0;
        LastLapTime = 0;
        CreateCars(true);
        RacingLine.Reset();
        spriteChanger.SetSprites(carObjects);
    }

    /// <summary>
    /// creates a list of cars and their associated agents and genomes
    /// as genomes must be made
    /// </summary>
    /// <param name="firstCreation">Boolean specifying if this is the first population created</param>
    /// <exception cref="System.Exception">prefab doesn't contain collision detector script </exception>
    private void CreateCars(bool firstCreation)
    {
        carObjects = new GameObject[carCount];
        cars = new Car[carCount];
        agents = new Agent[carCount];
        if (firstCreation)
            populationGenomes = new Genome[carCount];

        for (int i = 0; i < carCount; i++)
        {
            GameObject newCar = Instantiate(CarPrefab);
            carObjects[i] = newCar;
            CollisionDetector collisionDetector = newCar.GetComponent<CollisionDetector>();
            if (collisionDetector == null)
                throw new System.Exception("prefab does not contain collision detector");

            //initial position
            newCar.transform.position = startingPosition;
            newCar.transform.Rotate(startingRotation);
            //creates car and assigns to the gameObject
            newCar.AddComponent<Car>();

            Car car = newCar.GetComponent<Car>();
            car.ResetCar(maxTurnAngle, maxSpeed, maxAcceleration);
            cars[i] = car;

            CreateAgent(newCar, firstCreation, car, collisionDetector, i);
        }
    }

    /// <summary>
    /// Creates an agent for the current car
    /// </summary>
    /// <param name="newCar">GameObject of the car</param>
    /// <param name="firstCreation">Is this the first generation</param>
    /// <param name="car">Car script of this car</param>
    /// <param name="collisionDetector">Collision detector for this car</param>
    /// <param name="i">index of this car in relation to CarCount</param>
    private void CreateAgent(GameObject newCar, bool firstCreation, Car car, CollisionDetector collisionDetector, int i)
    {
        Agent agent = newCar.AddComponent<Agent>();
        agents[i] = agent;
        if (firstCreation)
        {
            //creates an instance of agent
            agent.Init(car, layerShaping, collisionDetector);
            //creates initial genome for this member of population
            Genome genome = new Genome(agent.neuralNetwork.GetParameters());
            agent.SetGenome(genome);
            populationGenomes[i] = genome;
        }
        else
        {
            agent.Init(car, populationGenomes[i], layerShaping, collisionDetector);
        }
    }

    /// <summary>
    /// Destroys all cars to be reset
    /// </summary>
    private void ResetCars()
    {
        for (int i = 0; i < carCount; i++)
        {
            populationGenomes[i].Fitness = 0;
            Destroy(carObjects[i]);
        }

        //prevents accessing null elements
        carObjects = new GameObject[0];
        cars = new Car[0];
        agents = new Agent[0];
    }

    /// <summary>
    /// Calls all necessary functions to reset the cars and begin the next generation
    /// </summary>
    private void ResetAll()
    {
        generationCount++;
        LastLapTime = LapTime;
        LapTime = 0;
        ResetCars();
        CreateCars(false);
        ResetPointsColliders();
        RacingLine.Reset();
        spriteChanger.SetSprites(carObjects);
    }

    /// <summary>
    /// Checks if all cars have crashed, or 2 cars have finished (if speeding up simulation), if so it starts the evolution process and then resets all cars
    /// </summary>
    private void Evolve()
    {
        if (Time.timeScale > 1f)
        {
            int finishedCount = 0;
            bool carRunning = false;
            for (int i = 0; i < carCount; i++)
            {
                if (agents[i].crashed == false & !ForceEvolve)
                    carRunning = true;
                if (agents[i].Finished == true)
                    finishedCount++;
            }
            if (carRunning == true && finishedCount < 2)
                return;
        }
        else
        {
            for (int i = 0; i < carCount; i++)
            {
                if (agents[i].crashed == false & !ForceEvolve)
                    return;
            }
        }
        populationGenomes = EvolutionManager.Evolve(populationGenomes);
        ResetAll();
        ForceEvolve = false;
    }

    /// <summary>
    /// small changes to fitness based on speed and time alive
    /// </summary>
    private void EvaluateFitness()
    {
        for (int i = 0; i < carCount; i++)
        {
            if (agents[i].crashed)
                continue;

            float speed = cars[i].Velocity;
            //inclined to move fast
            populationGenomes[i].Fitness += Mathf.Pow(speed, 1.3f) * 1f / Mathf.Pow(maxSpeed, 1.3f) * 0.1f;
            //inclined to not be alive too long (prevents a slow car from artificially increasing its score with the above increase in fitness)
            populationGenomes[i].Fitness -= Time.deltaTime * 0.1f;
        }
    }

    /// <summary>
    /// Resets the checkpoints scores
    /// </summary>
    private void ResetPointsColliders()
    {
        foreach (Score score in CheckpointScores)
        {
            score.Reset();
        }
    }

    /// <summary>
    /// Adds the position of the best car of the previous generation to the "racing line"
    /// </summary>
    /// <remarks>Allows you to see improvements in real time</remarks>
    private void DisplayLine()
    {
        RacingLine.AddPositions(carObjects[carCount - 2].transform.position);
        RacingLine.DisplayLine();

    }

    /// <summary>
    /// Returns the gameObject of the car with the highest fitness
    /// </summary>
    /// <returns>GameObject of fittest car</returns>
    private GameObject GetFittestCar()
    {

        float bestFitness = 0;
        GameObject bestCar = carObjects[0];
        for (int i = 0; i < carCount; i++)
        {
            if (bestFitness < populationGenomes[i].Fitness)
            {
                bestCar = carObjects[i];
                bestFitness = populationGenomes[i].Fitness;
            }
        }
        return bestCar;
    }

    /// <summary>
    /// Sets the sprite of the best car
    /// </summary>
    private void SpriteChanging()
    {
        GameObject bestCar = GetFittestCar();
        spriteChanger.setBestCar(bestCar);
    }

    /// <summary>
    /// Sets the layershaping of the neural networks
    /// </summary>
    /// <param name="LayerShaping">Topology</param>
    public void SetLayerShaping(uint[] LayerShaping)
    {
        this.layerShaping = LayerShaping;
    }

    /// <summary>
    /// Sets the number of cars
    /// </summary>
    /// <param name="Count">Will be changed to be within 300 and 2</param>
    public void SetCarCount(int Count)
    {
        if (Count<300 && Count >2)
        {
            carCount = Count;
        }
    }

    public void SetForceEvolve()
    {
        ForceEvolve = true;
    }

    private bool CarFinished()
    {
        for( int i = 0; i < carCount;i++)
        {
            if (agents[i].Finished)
                return true;
        }
        return false;
    }

    private void SetText()
    {
        GenerationText.text = "Generation: " + generationCount.ToString();
        LapTimeText.text = "Lap Time: " + LapTime.ToString();
        LastLapTimeText.text = "Last Lap Time: " + LastLapTime.ToString();
    }
    #endregion
}
