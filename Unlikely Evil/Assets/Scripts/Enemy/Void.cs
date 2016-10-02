using UnityEngine;
using System.Collections;

public class Void : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (player.StartGame && !player.IsDead())
        {
            Death dth = player.GetComponent<Death>();
            dth.StartCoroutine(dth.Kill());

            //Pull the player down when he steps into the void...
            player.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.Impulse);
        }
    }

}
