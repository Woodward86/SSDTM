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


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded; 
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }


    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu")
        {
            isStarted = true;
            Invoke("SpawnPlayer", .1f);
        }
    }


    private void SpawnPlayer()
    {
        if (isStarted)
        {
            if (numberOfPlayers > 0)
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    GameObject playerClone = Instantiate(player, player.transform.position, Quaternion.identity);

                    int playerNumber = i + 1;

                    playerClone.GetComponent<Player>().jumpButton = "Jump_P" + playerNumber.ToString();
                    playerClone.GetComponent<Player>().horizontalCtrl = "Horizontal_P" + playerNumber.ToString();
                }
            }
        }
    }

}
