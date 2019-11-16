using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject characterGroup;

    public void SpawnPlayer(bool isStarted, int numberOfPlayers, GameObject player, List<GameObject> players, GameObject[] spawnPoints)
    {
        characterGroup = GameObject.FindGameObjectWithTag("Character_Grp");

        if (isStarted)
        {
            if (numberOfPlayers > 0)
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    GameObject playerClone = Instantiate(player, spawnPoints[i].transform.position, Quaternion.identity, characterGroup.transform);

                    int playerNumber = i + 1;

                    //TODO this list of players will need to be done differently once there is a player select screen
                    players.Add(playerClone);

                    playerClone.GetComponent<PlayerController>().jumpButton = "Jump_P" + playerNumber.ToString();
                    playerClone.GetComponent<PlayerController>().leftHorizontalCtrl = "L_Horizontal_P" + playerNumber.ToString();

                }
            }
        }
    }
}
