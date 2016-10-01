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
    public int CorridorLength = 20;
    public int TimesReseted = 0;

    private Difficulty difficulty = Difficulty.Default;
    private GameObject player;
    private List<GameObject> corridor;
    private List<byte> corridorJar;

    private System.Random rand;

    void Awake()
    {
        rand = new System.Random();
        player = GameObject.FindWithTag("Player");
        corridorJar = new List<byte>();

        FillJar((int)difficulty);
    }

    void Start()
    {
        InitCorridor();
    }

    void Update()
    {
       
        if (Input.GetButtonUp("Fire3"))
        {
            Debug.Log("Difficulty Raised");
            difficulty++;
            Debug.Log(difficulty);
            FillJar((int)difficulty);
        }

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
        corridor = new List<GameObject>();
       
        GameObject a = Instantiate(Resources.Load("Corridor/Corridor1", typeof(GameObject)), new Vector3(-5, 0, -10), Quaternion.identity) as GameObject;
        corridor.Add(a);

        for (int i = 0; i < CorridorLength; i++)
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
    /// Should only be called on Reset in the player class. 
    /// Else the difficulty mechanic will break.
    /// </summary>
    public void ResetAccordingToPlayer()
    {
        if (gameObject.GetComponent<Movement>().StartGame)
        {
            TimesReseted++;

            difficulty++;
            FillJar((int)difficulty);
        }

        for (int i = 0; i < corridor.Count; i++)
        {
            corridor[i].transform.position = new Vector3(corridor[i].transform.position.x, corridor[i].transform.position.y, corridor[i].transform.position.z - ResetPoint);
        }

    }
    
    /// <summary>
    /// Sets the difficulty to the base level.
    /// </summary>
    public void ResetDifficulty()
    {
        difficulty = Difficulty.Default;
    }

    /// <summary>
    /// Appends a new corridor prefab to the existing corridor.
    /// </summary>
    private void AppendToRoad()
    {
        var corridorIndex = corridorJar[SelectRandomCorridor()];

        GameObject r = Instantiate(Resources.Load("Corridor/Corridor" + corridorIndex, typeof(GameObject)), new Vector3(-5, 0, Maxz() + 10), Quaternion.identity) as GameObject;

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
        return rand.Next(1, corridorJar.Count);
    }

    /// <summary>
    /// Imagine that we have an empty jar (toy box)
    /// and we fill it with different corridor prefabs(toys).
    /// </summary>
    private void FillJar(int availableCorridors)
    {
        corridorJar.Clear();

        byte corridor = 1;
        int divisor = availableCorridors - 1;

        while (divisor > 0)
        {
            int availableSpots = divisor * 10;

            for (int i = 0; i < availableSpots; i++)
            {
                if (divisor == availableCorridors - 2)
                    corridorJar.Add((byte)rand.Next(2, 4));

                corridorJar.Add(corridor);
            }

            if (divisor == availableCorridors - 2)
                corridor += 2;
            else
                corridor++;

            divisor--;
        }
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


/// <summary>
/// Controls the difficulty of the game
/// </summary>
public enum Difficulty
{
    Default = 2,
    Priest = 3,
    Angel = 4,
    //Jesus = 5,
    //God = 6,
}