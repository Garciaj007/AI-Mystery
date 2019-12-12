using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10f);

        if (GUILayout.Button("Train Neural Network") && Application.isPlaying && Application.isEditor)
            GameManager.Instance.Train();

        if (GUILayout.Button("Begin Simulation") && Application.isPlaying && Application.isEditor)
            GameManager.Instance.BeginSimulation();

        if (GUILayout.Button("Restart Simulation") && Application.isPlaying && Application.isEditor)
            GameManager.Instance.ResetSimulation();
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameObject[] players = null;

    [SerializeField] private List<Transform> randomPoints = new List<Transform>();
    [SerializeField] private GameObject objectivePrefab = null;

    private GameObject objective = null;

    public bool IsObjectiveActive { get; private set; } = false;
    public List<Transform> RandomPoints { get => randomPoints; }


    public void Train()
    {
        foreach (var player in players) player.GetComponent<PlayerController>().Train();
    }

    public void BeginSimulation()
    {
        foreach (var player in players) player.GetComponent<Animator>().SetTrigger("Simulate");
    }

    public void ResetSimulation() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public static GameObject GetClosestPlayer(GameObject currentPlayer)
    {
        var closestPlayer = players[0];
        foreach (var player in players)
        {
            if (player == currentPlayer) continue;
            if (currentPlayer.GetComponent<PlayerController>().IsSeeker && player.GetComponent<PlayerController>().IsSeeker) continue;
            if (!currentPlayer.GetComponent<PlayerController>().IsSeeker && !player.GetComponent<PlayerController>().IsSeeker) continue;

            var playerADistance = Vector3.Distance(currentPlayer.transform.position, player.transform.position);
            var playerBDistance = Vector3.Distance(currentPlayer.transform.position, closestPlayer.transform.position);
            if (playerADistance < playerBDistance)
                closestPlayer = player;
        }

        return closestPlayer;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        AssignRoles();
    }

    private void AssignRoles()
    {
        var rand = Random.Range(0, players.Length);
        for (var i = 0; i < players.Length; i++)
            players[i].GetComponent<PlayerController>().SetPlayerRole(rand == i);
    }

    public void SpawnObjective()
    {
        var rand = Random.Range(0, randomPoints.Count);
        objectivePrefab = Instantiate(objectivePrefab, randomPoints[rand].transform.position, Quaternion.identity);
    }

    public void HideObjective()
    {
        IsObjectiveActive = false;
        objective = null;
    }
}
