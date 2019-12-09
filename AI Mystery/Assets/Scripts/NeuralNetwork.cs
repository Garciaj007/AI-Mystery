using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Array = System.Array;
using Random = UnityEngine.Random;

/// <summary>
/// Simple Forward Neural Network
/// </summary>
public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private const float Bias = 0.25f; //bias
    private int[] layers; //layers
    private float[][] neurons; //neuron matix
    private float[][][] weights; //weight matrix
    private float fitness;

    public void AddFitness(float value) => fitness += value;
    public void SetFitness(float value) => fitness = value;
    public float GetFitness() => fitness; 

    /// <summary>
    /// Initializes Neural Network with random weights
    /// </summary>
    /// <param name="layers">layers of the Neural Network</param>
    public NeuralNetwork(int[] layers)
    {
        Array.ConstrainedCopy(layers, 0, this.layers, 0, layers.Length); 
        InitializeNeurons();
        InitializeWeights();
    }

    /// <summary>
    /// Deep copy constructor
    /// </summary>
    /// <param name="otherNetwork">Neural Network to copy from</param>
    public NeuralNetwork(NeuralNetwork otherNetwork)
    {
        Array.ConstrainedCopy(otherNetwork.layers, 0, this.layers, 0, otherNetwork.layers.Length);
        Array.ConstrainedCopy(otherNetwork.neurons, 0, this.neurons, 0, otherNetwork.neurons.Length);
        Array.ConstrainedCopy(otherNetwork.weights, 0, this.weights, 0, otherNetwork.weights.Length);
    }

    public int CompareTo(NeuralNetwork otherNetwork)
    {
        if (otherNetwork == null) return 1;
        if (fitness > otherNetwork.fitness) return 1;
        if (fitness < otherNetwork.fitness) return -1;
        return 0;
    }

    /// <summary>
    /// Creates Neuron Matrix
    /// </summary>
    public void InitializeNeurons()
    {
        neurons = layers.Select(layer => new float[layer]).ToArray();
    }

    /// <summary>
    /// Creates Neuron Weights Matrix
    /// </summary>
    private void InitializeWeights()
    {
        var weightsList = new List<float[][]>();
        for (var i = 1; i < layers.Length; i++)
        {
            var layerWeightsList = new List<float[]>();
            var neuronsInPreviousLayer = layers[i - 1];
            for (var j = 0; j < neurons[i].Length; j++)
            {
                var neuronWeights = new float[neuronsInPreviousLayer];
                
                for (var k = 0; k < neuronsInPreviousLayer; k++)
                    neuronWeights[k] = Random.Range(-0.5f, 0.5f);

                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    /// <summary>
    /// Feed forward inputs into the Neural Network
    /// </summary>
    /// <param name="inputs">Neural Network's Input DataSet</param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        //Adding inputs to the Neuron Matrix
        for (var i = 0; i < inputs.Length; i++)
            neurons[0][i] = inputs[i];

        //Iterating through all the neurons & computing feed-forward data
        for (var i = 1; i < layers.Length; i++)
        {
            for (var j = 0; j < neurons[i].Length; j++)
            {
                var value = Bias;
                for (var k = 0; k < neurons[i - 1].Length; k++)
                    value = weights[i - 1][j][k] * neurons[i - 1][k]; //Sum of all weights + previous activation functions
                neurons[i][j] = Mathf.Max(0.0f, value); //ReLU Activation
            }
        }
        return neurons[neurons.Length - 1]; // Return Output Array
    }

    public void Mutate()
    {
        foreach (var layers in weights)
        {
            foreach (var neuron in layers)
            {
                for (var k = 0; k < neuron.Length; k++)
                {
                    var weight = neuron[k];
                    var rand = Random.Range(0.0f, 10.0f);

                    if (rand <= 2f)
                        weight *= -1f;
                    else if(rand <= 4f)
                        weight = Random.Range(-0.5f, 0.5f);
                    else if (rand <= 6f)
                        weight *= Random.Range(0f, 1f) + 1f;
                    else if (rand <= 8f)
                        weight *= Random.Range(0f, 1f);

                    neuron[k] = weight;
                }
            }
        }
    }
}
