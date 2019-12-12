using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]

    public struct NeuralCommunicator
    {
        [System.Serializable]
        public struct NeuralDataset
        {
            public float[] expectedInputDataset;
            public float[] expectedOutputDataset;
        }

        public string name;
        public int trainingIterations;
        public List<int> layerSizes;
        public List<NeuralDataset> neuralDatasets;
        public BNeuralNetwork neuralNetwork;
    }

    [SerializeField] private Transform chaseEvadeTransform = null;
    [SerializeField] private bool isSeeker = false;
    [SerializeField] private bool canSeeEnemy = false;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float staminaDeflationRate = 0.1f;
    [SerializeField] public List<NeuralCommunicator> neuralCommunicators = new List<NeuralCommunicator>();

    private Unit aStarUnit = null;
    private ChaseAndEvade chaseAndEvade = null;

    private Animator animator = null;
    public bool IsSeeker { get => isSeeker; set => isSeeker = value; }
    public bool CanSeeEnemy { get => canSeeEnemy; }
    public float Stamina { get; private set; } = 1.0f;
    public float Speed { get => speed; }
    public GameObject ClosestPlayer { get; private set; } = null;

    #region Murder Mystery Deprecated Props
    public bool IsDead { get; private set; }
    public bool IsMurderer { get; private set; } = false;
    #endregion

    public void SetPlayerSeen(PlayerController otherPlayer, bool isInCollider) => canSeeEnemy = ((otherPlayer.IsSeeker && !IsSeeker) || (!otherPlayer.IsSeeker && IsSeeker) && isInCollider );

    public void SetPlayerRole(bool isSeeker)
    {
        IsSeeker = isSeeker;
        if (IsSeeker)
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        else
            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < neuralCommunicators.Count; i++)
        {
            var nc = neuralCommunicators[i];
            nc.neuralNetwork = new BNeuralNetwork(nc.layerSizes.ToArray());
            neuralCommunicators[i] = nc;
        }
    }

    public void Train()
    {
        neuralCommunicators.ForEach((nc) =>
        {
            for (int i = 0; i < nc.trainingIterations; i++)
            {
                nc.neuralDatasets.ForEach((data) =>
                {
                    nc.neuralNetwork.FeedForward(data.expectedInputDataset);
                    nc.neuralNetwork.BackProp(data.expectedOutputDataset);
                });
            }
        });

        Debug.ClearDeveloperConsole();
        neuralCommunicators.ForEach((nc) =>
        {
        Debug.Log($"------------------ { nc.name } ------------------");
            nc.neuralDatasets.ForEach((data) =>
            {
                var output = nc.neuralNetwork.FeedForward(data.expectedInputDataset);
                var debugString = "OutputValues: ";
                foreach (var val in output)
                    debugString += val.ToString();
                Debug.Log(debugString);
            });
        });
    }

    private void Update()
    {
        var distance = -1.0f;
        var action = 0.0f;

        if (CanSeeEnemy)
        {
            UpdateClosestPlayer();
            distance = Vector3.Distance(transform.position, ClosestPlayer.transform.position);
        }

        var inputDataSet = new float[] { Utils.Mathf.Bool2Float(IsSeeker), Utils.Mathf.Bool2Float(CanSeeEnemy), Utils.Mathf.Bool2Float(GameManager.Instance.IsObjectiveActive) };

        var inputDebugString = "Input Values: ";
        foreach(var data in inputDataSet)
        {
            inputDebugString += data.ToString();
        }
        Debug.Log(inputDebugString);

        neuralCommunicators.ForEach((nc) => { action = nc.neuralNetwork.FeedForward(inputDataSet)[0]; });
        animator.SetFloat("Action", action);
    }

    public void Kill() => IsDead = true;
    public void SetMurder(bool isMurderer) => IsMurderer = isMurderer;
    public void UpdateClosestPlayer() => ClosestPlayer = GameManager.GetClosestPlayer(gameObject);
    public void UseStamina()
    {
        Stamina -= staminaDeflationRate;
        Stamina = Stamina < 0.0f ? 0.0f : Stamina;
    }
}
