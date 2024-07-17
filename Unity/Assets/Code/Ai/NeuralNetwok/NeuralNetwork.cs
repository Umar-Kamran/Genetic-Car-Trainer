using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NeuralNetwork
{
    #region Attributes

    /// <summary>
    /// Layers of this network
    /// </summary>
    public NNLayer[] Layers {  get; private set; }

    /// <summary>
    /// Stores the number of neurons per layer
    /// </summary>
    public uint[] LayerShaping { get; private set; }

    #endregion

    #region Constructor

    public NeuralNetwork(uint[] layerShaping)
    {
        this.LayerShaping = layerShaping;
        Layers = new NNLayer[layerShaping.Length - 1];

        for (int i = 0; i < layerShaping.Length - 1; i++)
            Layers[i] = new NNLayer(layerShaping[i], layerShaping[i+1]);
        
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

        for (int i = 0; i <= Layers.Length; i++)
            input = Layers[i].GenerateOutputs(input);

        return input;
    }

    public void ResetLayers(double range, double shift)
    {
        foreach (NNLayer layer in Layers)
        {
            layer.InitialiseLayerValues(range, shift);
        }
    }
    #endregion
}
