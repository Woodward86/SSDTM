using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelController : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject killPlane;

    public void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }


    public void GetKillPlane()
    {
        killPlane = GameObject.FindGameObjectWithTag("Kill_Plane");
    }
}
