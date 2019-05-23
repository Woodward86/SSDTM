using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonFunctions : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject mainMenu;
    private GameObject settingsMenu;


    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        mainMenu = GameObject.FindGameObjectWithTag("Main Menu");
        settingsMenu = GameObject.FindGameObjectWithTag("Settings Menu");
    }
 

    public void SinglePlayerGame()
    {
        Debug.Log("Launching single player game");
        gameManager.GetComponent<PlayerSpawner>().numberOfPlayers = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void MultiPlayerGame()
    {
        Debug.Log("Launching multi player game");
        gameManager.GetComponent<PlayerSpawner>().numberOfPlayers = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void SettingsMenu()
    {
        Debug.Log("Launching the settings menu");
        // Can't use SetActive this way.  If the menu is not active you cannot access this attribute
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }


    public void GoBack()
    {
        Debug.Log("Going to the previous menu");
        // Can't use SetActive this way.  If the menu is not active you cannot access this attribute
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    public void ExitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}


