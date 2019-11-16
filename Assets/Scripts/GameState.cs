using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(PlayerSpawner))]
[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(LevelController))]
public class GameState : MonoBehaviour
{
    protected LevelController levelController;
    protected PlayerSpawner playerSpawner;
    protected CameraController cameraController;

    public List<GameObject> players = new List<GameObject>();
    public int numberOfPlayers;
    public GameObject player;
    private bool isStarted = false;

    public List<GameObject> characters = new List<GameObject>();


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        levelController = GetComponent<LevelController>();
        playerSpawner = GetComponent<PlayerSpawner>();
        cameraController = GetComponent<CameraController>();
        SceneManager.sceneLoaded += OnLevelLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }


    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        cameraController.followPlayer = false;
        cameraController.mainCamera = Camera.main;

        if (scene.name == "CharacterSelect")
        {
            isStarted = true;
            levelController.GetSpawnPoints();
            playerSpawner.SpawnPlayer(isStarted, numberOfPlayers, player, players, levelController.spawnPoints);
        }
        if (scene.name != "StartMenu" && scene.name != "CharacterSelect")
        {
            cameraController.followPlayer = true;
        }

    }
}
