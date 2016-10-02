using UnityEngine;
using System.Collections;

public class Hunter : MonoBehaviour
{
    public float Speed;
    public int BeginHuntDelay = 1;

    private Player player;
    private bool beginHunt;
    private int startIndex = 0;       // if the start index goes to 2 start the hunt

    void Start ()
    {
        beginHunt = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    /// <summary>
    /// Delays the hunter.
    /// </summary>
    /// <returns></returns>
    IEnumerator HuntDelay()
    {
        yield return new WaitForSeconds(BeginHuntDelay);
        beginHunt = true;
    }

    void Update()
    {
        if (player != null)
        {
            StartAfterInput(player.StartGame);
            HuntPlayer(Speed, beginHunt);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !player.IsDead())
        {
            StartCoroutine(player.death.Kill());
        }
    }

    /// <summary>
    /// Begins the hunt.
    /// </summary>
    /// <param name="speed">Speed of movement</param>
    /// <param name="beginHunt">Indicating whether to start the hunt</param>
    private void HuntPlayer(float speed, bool beginHunt)
    {
        if (beginHunt)
        {
            float huntSpeed = speed;

            if (huntSpeed >= player.movement.Speed)
                huntSpeed = speed;
            else
                huntSpeed = (player.movement.Speed - speed);


            Vector3 hunt = new Vector3(0, 0, huntSpeed * Time.deltaTime);
            transform.Translate(hunt, Space.World);
        }
    }

    /// <summary>
    /// Starts the hunt only after the player has given a movement input.
    /// </summary>
    private void StartAfterInput(bool startGame)
    {
        if (Input.GetButtonUp("Fire1") && startGame)
            startIndex++;

        if (startIndex == 2)
            StartCoroutine(HuntDelay());
    }

    /// <summary>
    /// Resets the hunter and gets ready for the restart 
    /// </summary>
    /// <param name="resetPosition"></param>
    public void Reset(Vector3 resetPosition)
    {
        if (player.StartGame)
        {
            beginHunt = false;
            startIndex = 0;
            transform.position = resetPosition;
        }
    }
}
