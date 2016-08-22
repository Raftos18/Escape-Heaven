using UnityEngine;
using System.Collections.Generic;

public class roadGenerator : MonoBehaviour
{
    public int ResetPoint = 5000;
    public int numberOfPrefabs = 4;
    public int roadLength = 7;
    public int TimesReseted;

    private GameObject player;
    private List<GameObject> road;

    private System.Random rand;

    void Start()
    {
        rand = new System.Random();
        road = new List<GameObject>();
        player = GameObject.FindWithTag("Player");
        GameObject a = Instantiate(Resources.Load("road/roadPlane", typeof(GameObject)), new Vector3(5, 0, 0), Quaternion.identity) as GameObject;
        GameObject b = Instantiate(Resources.Load("road/roadPlane", typeof(GameObject)), new Vector3(5, 0, maxz() + 10), Quaternion.identity) as GameObject;
        GameObject c = Instantiate(Resources.Load("road/roadPlane", typeof(GameObject)), new Vector3(5, 0, maxz() + 10), Quaternion.identity) as GameObject;

        road.Add(a);
        road.Add(b);
        road.Add(c);

        for (int i = 0; i < roadLength; i++)
        {
            appendToRoad();
        }
    }

    void Update()
    {
            player = GameObject.FindWithTag("Player");
            for (int i = 0; i < road.Count; i++)
            {
                if (road[i].transform.position.z + 10 < player.transform.position.z)
                {
                    GameObject.Destroy(road[i]);
                    road.Remove(road[i]);
                    appendToRoad();
                }
            }
    }

    public void resetAccordingToPlayer()
    {
        if(gameObject.GetComponent<Movement>().StartGame)
            TimesReseted++;

        for (int i = 0; i < road.Count; i++)
        {
            road[i].transform.position = new Vector3(road[i].transform.position.x, road[i].transform.position.y, road[i].transform.position.z - 5000);
        }

    }

    int index = 0;
    private void appendToRoad()
    {
        string road_part_index = rand.Next(0, numberOfPrefabs).ToString();
        if (road_part_index == "0")
            road_part_index = "";
        GameObject r = Instantiate(Resources.Load("road/roadPlane" + road_part_index, typeof(GameObject)),new Vector3(-5,0,maxz()+10),Quaternion.identity) as GameObject;

        //string trap_index = rand.Next(0, numberOfPrefabs).ToString();
        //if (trap_index != "0")
        //{
        //    GameObject tr = Instantiate(Resources.Load("traps/trap" + trap_index, typeof(GameObject))) as GameObject;
        //    tr.transform.position = new Vector3(tr.transform.position.x, transform.position.y, r.transform.position.z);
        //    tr.transform.parent = r.transform;
        //}

        road.Add(r);
    }

    private float maxz()
    {
        float max = 0;
        foreach (GameObject g in road)
        {
            if (g.transform.position.z > max)
                max = g.transform.position.z;
        }
        return max;
    }
}
