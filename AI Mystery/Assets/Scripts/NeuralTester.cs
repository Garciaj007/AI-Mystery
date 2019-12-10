using UnityEngine;

public class NeuralTester : MonoBehaviour
{
    private void Start()
    {
        var bNet = new BNeuralNetwork(new int[]{3, 25, 25, 1});

        for (var i = 0; i < 5000; i++)
        {
            bNet.FeedForward(new float[] { 0, 0, 0 });
            bNet.BackProp(new float[]{ 0 });

            bNet.FeedForward(new float[] { 0, 0, 1 });
            bNet.BackProp(new float[]{ 1 });

            bNet.FeedForward(new float[] { 0, 1, 0 });
            bNet.BackProp(new float[]{ 1 });

            bNet.FeedForward(new float[] { 0, 1, 1 });
            bNet.BackProp(new float[]{ 0 });

            bNet.FeedForward(new float[] { 1, 0, 0 });
            bNet.BackProp(new float[]{ 1 });

            bNet.FeedForward(new float[] { 1, 0, 1 });
            bNet.BackProp(new float[]{ 0 });

            bNet.FeedForward(new float[] { 1, 1, 0 });
            bNet.BackProp(new float[]{ 0 });

            bNet.FeedForward(new float[] { 1, 1, 1 });
            bNet.BackProp(new float[]{ 1 });
        }

        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{0,0,0})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{0,0,1})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{0,1,0})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{0,1,1})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{1,0,0})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{1,0,1})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{1,1,0})[0]));
        Debug.Log(Mathf.Round(bNet.FeedForward(new float[]{1,1,1})[0]));
    }
}
