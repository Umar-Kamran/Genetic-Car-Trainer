using System;
using System.Collections;

public class NeuralNetwork
{
    #region Attributes

    /// <summary>
    /// Layers of this network
    /// </summary>
    public NNLayer[] Layers { get; private set; }

    /// <summary>
    /// Stores the number of neurons per layer
    /// </summary>
    public uint[] LayerShaping { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor for a neural network. Creates each layer of this neural network using their constructors
    /// </summary>
    /// <param name="layerShaping">Topology of this neural network</param>
    public NeuralNetwork(uint[] layerShaping)
    {
        this.LayerShaping = layerShaping;
        Layers = new NNLayer[layerShaping.Length - 1];

        for (int i = 0; i < layerShaping.Length - 1; i++)
            Layers[i] = new NNLayer(layerShaping[i], layerShaping[i + 1]);

    }
    #endregion

    #region Methods
    /// <summary>
    /// Completes a full feedforward pass through all layers
    /// </summary>
    /// <param name="input">inputs to the first layer</param>
    /// <returns>Output after all layers</returns>
    /// <exception cref="ArgumentException">Length of input is not equal to the number of neurons in layer 0</exception>
    public double[] FeedForward(double[] input)
    {
        if (input.Length != Layers[0].InputNodes)
            throw new ArgumentException();

        for (int i = 0; i <= Layers.Length - 1; i++)
            input = Layers[i].GenerateOutputs(input);

        return input;
    }
    /// <summary>
    /// Creates initial values for the biases and weights of this layer. 
    /// Biases are set to 0.
    /// Weights are a random value from (shift -> range + shift)
    /// </summary>
    /// <param name="range">Range of values</param>
    /// <param name="shift">Smallest value</param>
    public void ResetLayers(double range, double shift)
    {
        foreach (NNLayer layer in Layers)
        {
            layer.InitialiseLayerValues(range, shift);
        }
    }

    /// <summary>
    /// Sets the weights and biases of each layer of this neural network using the inputted parameters
    /// </summary>
    /// <param name="Parameters">Weights and biases to assign. Array</param>
    public void SetParameters(double[] Parameters)
    {
        IEnumerator values = Parameters.GetEnumerator();
        foreach (NNLayer layer in Layers)
            for (int j = 0; j < layer.OutputNodes; j++)
            {
                values.MoveNext();
                layer.Biases[j] = (double)values.Current;
                for (int i = 0; i < layer.InputNodes; i++)
                {
                    values.MoveNext();
                    layer.Weights[i, j] = (double)values.Current;
                }
            }
    }

    /// <summary>
    /// Returns and array of all weights and biases of all layers of this neural network
    /// </summary>
    /// <returns>array of layers and biases flattened</returns>
    public double[] GetParameters()
    {
        uint count = 0;
        for (int i = 0; i < Layers.Length; i++)
            count += (LayerShaping[i] + 1) * LayerShaping[i + 1];

        double[] parameters = new double[count];
        uint pointer = 0;

        foreach (NNLayer layer in Layers)
            for (int j = 0; j < layer.OutputNodes; j++)
            {

                parameters[pointer] = layer.Biases[j];
                pointer++;
                for (int i = 0; i < layer.InputNodes; i++)
                {
                    parameters[pointer] = layer.Weights[i, j];
                    pointer++;
                }
            }
        return parameters;
    }
    #endregion
}
