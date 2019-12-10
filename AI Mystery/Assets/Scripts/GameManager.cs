using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10f);

        if(GUILayout.Button("Train Neural Network"))
        {
            foreach (var player in GameManager.players)
                player?.GetComponent<PlayerController>()?.Train();
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool IsObjectiveActive { get; set; }
    public static GameObject[] players = null;
    private static GameObject[] hiders = null;
    private static GameObject[] seekers = null;
    private static GameObject objective = null;

    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject objectivePrefab; 
    [SerializeField] private int NumberOfPlayersToSpawn = 10;

    public static GameObject GetClosestPlayer(GameObject currentPlayer)
    {
        if (hiders == null || currentPlayer == null) return null;

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
        hiders = GameObject.FindGameObjectsWithTag("Hiders");
        seekers = GameObject.FindGameObjectsWithTag("Seekers");
        players = new GameObject[hiders.Length + seekers.Length];
        hiders.CopyTo(players, 0);
        seekers.CopyTo(players, hiders.Length);
    }

    public void SpawnObjective()
    {
        var rand = Random.Range(0, spawnPoints.Count);
        objectivePrefab = Instantiate(objectivePrefab, spawnPoints[rand].transform.position, Quaternion.identity);
    }
}
