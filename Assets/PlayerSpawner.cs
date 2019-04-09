using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public int numberOfPlayers;
    public GameObject player;
    private bool isStarted = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void OnLevelWasLoaded()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "Menu")
        {
            isStarted = true;
            Invoke("SpawnPlayer", .1f);
        }
    }


    private void SpawnPlayer()
    {
        if (isStarted)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                int playerNumber = i + 1;

                GameObject playerClone = Instantiate(player, player.transform.position, Quaternion.identity);

                playerClone.GetComponent<PlayerController>().jumpButton = "Jump_P" + playerNumber.ToString();
                playerClone.GetComponent<PlayerController>().horizontalCtrl = "Horizontal_P" + playerNumber.ToString();
            }
        }
    }

}
