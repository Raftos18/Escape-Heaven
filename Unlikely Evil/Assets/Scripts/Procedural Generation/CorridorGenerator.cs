using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// This class controls the corridor generation.
/// Every time the corridor is reseted at the 
/// selected ResetPoint a new obstacle is added
/// (Currently only one). 
/// </summary>

public class CorridorGenerator : MonoBehaviour
{
    public int ResetPoint = 5000;
    public int numberOfPrefabs = 4;
    public int roadLength = 7;
    public int TimesReseted = 0;

    private GameObject player;
    private List<GameObject> corridor;
    private byte[] corridorJar;                     

    private System.Random rand;


    void Awake()
    {
        FillJar();
    }

    void Start()
    {
        InitCorridor();
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        for (int i = 0; i < corridor.Count; i++)
        {
            if (corridor[i].transform.position.z + 10 < player.transform.position.z)
            {
                GameObject.Destroy(corridor[i]);
                corridor.Remove(corridor[i]);
                AppendToRoad();
            }
        }
    }

    /// <summary>
    /// Initialize the corridor by adding the basic corridor prefab 4 times
    /// </summary>
    private void InitCorridor()
    {
        rand = new System.Random();
        corridor = new List<GameObject>();
        player = GameObject.FindWithTag("Player");

        GameObject a = Instantiate(Resources.Load("Corridor/Corridor1", typeof(GameObject)), new Vector3(-5, 0, -10), Quaternion.identity) as GameObject;
        GameObject b = Instantiate(Resources.Load("Corridor/Corridor1", typeof(GameObject)), new Vector3(-5, 0, Maxz()), Quaternion.identity) as GameObject;
        GameObject c = Instantiate(Resources.Load("Corridor/Corridor1", typeof(GameObject)), new Vector3(-5, 0, Maxz()), Quaternion.identity) as GameObject;

        corridor.Add(a);
        corridor.Add(b);
        corridor.Add(c);

        for (int i = 0; i < roadLength; i++)
            AppendToRoad();
    }

    /// <summary>
    /// Resets the corridor on origin when the ResetPoint is reached.
    /// </summary>
    /// <param name="player"></param>
    public void ResetCorridorOnPoint(Movement player)
    {
        if (transform.position.z > gameObject.GetComponent<CorridorGenerator>().ResetPoint)
        {
            GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");
            Vector3 resetPos = new Vector3(0, 0.5f, -Math.Abs(transform.position.z - hunter.transform.position.z));
            player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            ResetAccordingToPlayer();
            hunter.GetComponent<Hunter>().Reset(resetPos);
        }
    }

    /// <summary>
    /// Resets the corridor according to the players position.
    /// </summary>
    public void ResetAccordingToPlayer()
    {
        if (gameObject.GetComponent<Movement>().StartGame)
            TimesReseted++;

        for (int i = 0; i < corridor.Count; i++)
        {
            corridor[i].transform.position = new Vector3(corridor[i].transform.position.x, corridor[i].transform.position.y, corridor[i].transform.position.z - 5000);
        }

    }

    /// <summary>
    /// Appends a new corridor prefab to the existing corridor.
    /// </summary>
    private void AppendToRoad()
    {
        var corridorIndex = corridorJar[SelectRandomCorridor()];

        GameObject r = Instantiate(Resources.Load("Corridor/Corridor" + corridorIndex, typeof(GameObject)),new Vector3(-5, 0, Maxz() + 10), Quaternion.identity) as GameObject;

        corridor.Add(r);
    }

    /// <summary>
    /// Selects a random corridor prefab.
    /// Corridor prefabs have different weights
    /// so some are more common than others.
    /// </summary>
    /// <returns>An integer index</returns>
    private int SelectRandomCorridor()
    {
        const int highCorChance = 10;

        if(TimesReseted < 1)
            return rand.Next(1, corridorJar.Length - highCorChance);
        else
            return rand.Next(1, corridorJar.Length);
    }

    /// <summary>
    /// Imagine that we have an empty jar (toy box)
    /// and we fill it with different corridor prefabs (toys).
    /// </summary>
    private void FillJar()
    {
        corridorJar = new byte[100];
        for (int i = 0; i < 100; i++)
            if (i < 50)
                corridorJar[i] = 1;
            else if (i < 80)
                corridorJar[i] = 2;
            else if (i < 90)
                corridorJar[i] = 3;
            else
                corridorJar[i] = 4;

    }

    /// <summary>
    /// Finds the end point of a corridor prefab.
    /// </summary>
    /// <returns></returns>
    private float Maxz()
    {
        float max = 0;
        foreach (GameObject g in corridor)
            if (g.transform.position.z > max)
                max = g.transform.position.z;
        
        return max;
    }
}
