using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class NNLayer 
{
    #region Attributes

    /// <summary>
    /// Weights of this layer.
    /// 2D array where Weights[i,j] represents the weight of 
    /// node i of this layer to node j of the next layer
    /// </summary>
    public double[,] Weights
    {  get; private set; }

    /// <summary>
    /// Biases of this layer.
    /// Bias[i] represents the bias added to generate
    /// the value of node i in the next layer
    /// </summary>
    public double[] Biases { get; private set;}

    /// <summary>
    /// Contains the activation function for this layer
    /// </summary>
    /// <param name="inpValue">Value which the function is applied to </param>
    /// <returns>The output of the Activation Function applied to the input</returns>
    public delegate double ActivationFunction(double inpValue);

    public ActivationFunction Activation = System.Math.Tanh;

    /// <summary>
    /// Stores number of nodes in this layer
    /// </summary>
    public uint InputNodes {  get; private set; }

    /// <summary>
    /// Stores number of outputs, so the number of nodes in the next layer
    /// </summary>
    public uint OutputNodes {  get; private set; }
    #endregion

    #region Constructor
    public NNLayer (uint inputNodes, uint outputNode)
    {
        InputNodes = inputNodes;
        OutputNodes = outputNode;
        
        Weights = new double[InputNodes, OutputNodes];
    }

    private static System.Random rnd = new System.Random();
    #endregion

    #region Methods
    /// <summary>
    /// Changes the weights of this layer to the inputted values
    /// </summary>
    /// <param name="weights">Values to change Layers weights to</param>
    public void SetWeights(double[,] weights)
    {
        int x = weights.GetLength(0);
        int y = weights.GetLength(1);

        if (x != InputNodes || y != OutputNodes)
            return;

        for (int i = 0; i < x; i++)
            for(int j = 0; j < y; j++)
                this.Weights[i,j] = weights[i, j];
    }

    /// <summary>
    /// Changes the Biases of this layer to the inputted values
    /// </summary>
    /// <param name="biases">Values to change Layers biases to</param>
    public void SetBiases(double[] biases)
    {
        int x = biases.Length;
        if (x != Biases.Length)
            return;

        for(int i = 0;i < x; i++)
            this.Biases[i] = biases[i];
    }

    /// <summary>
    /// Feeds the inputs, multiplies by the weight and adds a bias per node
    /// i.e. feedforward of 1 layer
    /// </summary>
    /// <param name="inputs">Inputs to this layer to have weights and biases applied to</param>
    /// <returns>Outputs of applying weights and biases</returns>
    public double[] GenerateOutputs(double[] inputs)
    {
        if(inputs.Length != Weights.GetLength(0))
            return null;

        double[] weightedValues = new double[OutputNodes];
        for (int j = 0; j < OutputNodes; j++)
        {
            weightedValues[j] = Biases[j];
            for (int i = 0; i < InputNodes; i++)
                weightedValues[j] += inputs[i] * Weights[i, j];

            if (Activation != null)
                weightedValues[j] = Activation(weightedValues[j]);

        }

        return weightedValues;
    }

    /// <summary>
    /// Creates a copy of this Layer as a deep copy (not a shallow copy) 
    /// </summary>
    /// <returns>Copy of this Layer</returns>
    public NNLayer Copy()
    {
        NNLayer copyLayer = new NNLayer(InputNodes, OutputNodes);

        double[,] tempWeights = new double[InputNodes, OutputNodes];
        double[] tempBias = new double[OutputNodes];
        for (int j = 0; j < OutputNodes; j++)
        {
            tempBias[j] = Biases[j];
            for (int i = 0; i < InputNodes; i++)
                tempWeights[i, j] = Weights[i, j];
        }

        copyLayer.SetBiases(tempBias);
        copyLayer.SetWeights(tempWeights);

        return copyLayer;
    }
    /// <summary>
    /// Creates initial values for the biases and weights of this layer. 
    /// Biases are set to 0.
    /// Weights are a random value from (shift -> range + shift)
    /// </summary>
    /// <param name="range">Range of values</param>
    /// <param name="shift">Smallest value</param>
    public void InitialiseLayerValues(double range, double shift)
    {

        
        for (int j = 0; j < OutputNodes; j++) {
            Biases[j] = 0;
            for (int i = 0; i <= InputNodes; i++)
                Weights[i, j] = rnd.NextDouble() * range + shift;
            }
    }
    #endregion
}
