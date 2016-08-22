using UnityEngine;
using System.Collections;

public class Void : MonoBehaviour
{
    Movement player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (player.StartGame && !player.isDead)
        {
            Death dth = player.GetComponent<Death>();
            dth.StartCoroutine(dth.Kill());
        }
    }

}
