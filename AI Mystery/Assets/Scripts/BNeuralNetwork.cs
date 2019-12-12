using UnityEngine;
using Math = System.Math;

public class BNeuralNetwork
{
    public class Layer
    {
        private const float LearningRate = 0.33f;
        private int numberOfInputs;
        private int numberOfOutputs;

        public float[] outputs;
        public float[] inputs;
        public float[] gamma;
        public float[] error;
        public float[,] weights;
        public float[,] weightsDelta;

        public Layer(int numberOfInputs, int numberOfOutputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            error = new float[numberOfOutputs];
            gamma = new float[numberOfOutputs];
            inputs = new float[numberOfInputs];
            outputs = new float[numberOfOutputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            weightsDelta = new float[numberOfOutputs, numberOfInputs];

            InitializeWeights();
        }

        public void InitializeWeights()
        {
            for (var i = 0; i < numberOfOutputs; i++)
                for (var j = 0; j < numberOfInputs; j++)
                    weights[i, j] = Random.Range(-0.5f, 0.5f);
        }

        public float[] FeedForward(float[] inputs)
        {
            this.inputs = inputs;

            for (var i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;

                for (var j = 0; j < numberOfInputs; j++)
                    outputs[i] += inputs[j] * weights[i, j];

                outputs[i] = (float)Math.Tanh(outputs[i]);
            }
            return outputs;
        }

        public float TanHDer(float value)
        {
            return 1 - (value * value);
        }

        public void BackPropOutput(float[] expected)
        {
            for (var i = 0; i < numberOfOutputs; i++)
                error[i] = outputs[i] - expected[i];

            for (var i = 0; i < numberOfOutputs; i++)
                gamma[i] = error[i] * TanHDer(outputs[i]);

            for (var i = 0; i < numberOfOutputs; i++)
                for (var j = 0; j < numberOfInputs; j++)
                    weightsDelta[i, j] = gamma[i] * inputs[j];
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
        {
            for (var i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;

                for (var j = 0; j < gammaForward.Length; j++)
                    gamma[i] += gammaForward[j] * weightsForward[j, i];

                gamma[i] *= TanHDer(outputs[i]);
            }

            for (var i = 0; i < numberOfOutputs; i++)
                for (var j = 0; j < numberOfInputs; j++)
                    weightsDelta[i, j] = gamma[i] * inputs[j];
        }

        public void UpdateWeights()
        {
            for (var i = 0; i < numberOfOutputs; i++)
                for (var j = 0; j < numberOfInputs; j++)
                    weights[i, j] -= weightsDelta[i, j] * LearningRate;
        }
    }

    private int[] layerSizes; //layers
    private Layer[] layers;

    public BNeuralNetwork(int[] layerSizes)
    {
        this.layerSizes = new int[layerSizes.Length];
        for(var i = 0; i < layerSizes.Length; i++)
            this.layerSizes[i] = layerSizes[i];

        layers = new Layer[layerSizes.Length - 1];
        for (var i = 0; i < layers.Length; i++)
            layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
    }

    public float[] FeedForward(float[] inputs)
    {
        layers[0].FeedForward(inputs);
        for (var i = 1; i < layers.Length; i++)
            layers[i].FeedForward(layers[i - 1].outputs);

        return layers[layers.Length - 1].outputs;
    }

    public void BackProp(float[] expected)
    {
        for (var i = layers.Length - 1; i >= 0; i--)
        {
            if (i == layers.Length - 1)
                layers[i].BackPropOutput(expected);
            else
                layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights);
        }

        foreach (var layer in layers) layer.UpdateWeights();
    }
}
