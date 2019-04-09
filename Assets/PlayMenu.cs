using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    private GameObject gameManager;


    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
    }


    public void SinglePlayerGame()
    {
        gameManager.GetComponent<PlayerSpawner>().numberOfPlayers = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void TwoPlayerGame()
    {
        gameManager.GetComponent<PlayerSpawner>().numberOfPlayers = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
