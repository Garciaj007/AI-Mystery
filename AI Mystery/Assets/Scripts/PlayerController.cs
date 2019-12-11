using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]

    struct NeuralCommunicator
    {
        public string name;
        public BNeuralNetwork neuralNetwork;
        public List<int> layerSizes;
    }


    public bool IsSeeker { get => isSeeker; set => isSeeker = value; }
    public bool CanSeeEnemy { get; set; } = false;

    public bool IsDead { get; private set; }
    public bool IsMurderer { get; private set; } = false;

    public float Stamina { get; private set; } = 1.0f;
    public float Speed { get => speed; }
    public GameObject ClosestPlayer { get; private set; } = null;

    [SerializeField] private Transform target;
    [SerializeField] private bool isSeeker;
    [SerializeField] private float speed;
    [SerializeField] private float staminaDeflationRate;
    [SerializeField] private List<NeuralCommunicator> neuralCommunicators = new List<NeuralCommunicator>();

    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < neuralCommunicators.Count; i++)
        {
            var nc = neuralCommunicators[i];
            nc.neuralNetwork = new BNeuralNetwork(nc.layerSizes.ToArray());
            neuralCommunicators[i] = nc;
        }

        Train();
    }

    public void Train()
    {
        for (int i = 0; i < 500; i++)
        {
            neuralCommunicators.ForEach((nc) =>
            {
                nc.neuralNetwork.FeedForward(new float[] { 2.0f, Bool2Float(false), Bool2Float(true), Bool2Float(false) });
                nc.neuralNetwork.BackProp(new float[] {2.0f});

                nc.neuralNetwork.FeedForward(new float[] { 2.0f, Bool2Float(true), Bool2Float(true), Bool2Float(false) });
                nc.neuralNetwork.BackProp(new float[] { 1.0f });

                //nc.neuralNetwork.FeedForward(new float[] { 3.0f, Bool2Float(false), Bool2Float(false), Bool2Float(true) });
                //nc.neuralNetwork.BackProp(new float[] { 3.0f });

                nc.neuralNetwork.FeedForward(new float[] { 5.0f, Bool2Float(true), Bool2Float(false), Bool2Float(false) });
                nc.neuralNetwork.BackProp(new float[] { 0.0f });

                nc.neuralNetwork.FeedForward(new float[] { 5.0f, Bool2Float(false), Bool2Float(false), Bool2Float(false) });
                nc.neuralNetwork.BackProp(new float[] { 0.0f });
            });
        }

        Debug.ClearDeveloperConsole();
        neuralCommunicators.ForEach((nc) => 
        {
        Debug.Log(Mathf.Round(nc.neuralNetwork.FeedForward(new float[]{ 2.0f, Bool2Float(false), Bool2Float(true), Bool2Float(false)})[0]));
        Debug.Log(Mathf.Round(nc.neuralNetwork.FeedForward(new float[]{Bool2Float(true), Bool2Float(true), Bool2Float(false)})[0]));
        Debug.Log(Mathf.Round(nc.neuralNetwork.FeedForward(new float[]{Bool2Float(true), Bool2Float(false), Bool2Float(false)})[0]));
        Debug.Log(Mathf.Round(nc.neuralNetwork.FeedForward(new float[]{5.0f, Bool2Float(false), Bool2Float(false), Bool2Float(false)})[0]));
        });
    }

    private void Update()
    {
        //ClosestPlayer = GameManager.GetClosestPlayer(gameObject);
        //var distance = Vector3.Distance(transform.position, GameManager.GetClosestPlayer(gameObject).transform.position);

        var distance = Vector3.Distance(transform.position, target.position);
        float[] inputDataSet = { distance, Bool2Float(IsSeeker), Bool2Float(CanSeeEnemy), Bool2Float(GameManager.IsObjectiveActive) };
        var action = 0;
        neuralCommunicators.ForEach((nc) => { action = Mathf.RoundToInt(nc.neuralNetwork.FeedForward(inputDataSet)[0]); });
        animator.SetInteger("Action", action);
    }

    public void Kill() => IsDead = true;
    public void SetMurder(bool isMurderer) => IsMurderer = isMurderer;

    public static float Bool2Float(bool boolean) => boolean ? 1 : 0;

    public void UseStamina()
    {
        Stamina -= staminaDeflationRate;
        Stamina = Stamina < 0.0f ? 0.0f : Stamina;
    }
}
