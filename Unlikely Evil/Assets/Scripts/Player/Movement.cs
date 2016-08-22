using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Controls the movement of the player character
/// Notes:
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float TurnSpeed;
    public int MaxSpeed;
    public float Acceleration;
    public float Deceleration;
    public float DecelerationCooldown;

    [Header("Floating")]
    public float Amplitude = 0.5f;
    public float FloatSpeed = 0.5f;
    public float floatOffset = 1.5f;

    [Space]
    public bool StartGame;
    public AudioClip[] Sounds;
    public bool isDead;

    private Rigidbody rb;
    private AudioSource ad;
    private float minSpeed;
    private float minDeceleration;
    private const float speedMultiplier = 10.0f;                        // Simple Multiplier to keep numbers smaller.
   
	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ad = gameObject.GetComponent<AudioSource>();
        minSpeed = Speed;
        minDeceleration = Deceleration;
        StartGame = false;
        isDead = false;
    }

    void FixedUpdate()
    {
        if (StartGame)
            Move();
        else
            OpeningMove();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayDeathSound(Sounds[(int)SoundsEnum.Death]);
        if (transform.position.z > gameObject.GetComponent<roadGenerator>().ResetPoint)
        {
            GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");
            Vector3 resetPos = new Vector3(0, 0.5f, -Math.Abs(transform.position.z - hunter.transform.position.z));
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            gameObject.GetComponent<roadGenerator>().resetAccordingToPlayer();
            hunter.GetComponent<Hunter>().Reset(resetPos);
        }
    }

    /// <summary>
    /// Simple straight movement for the opening of the game
    /// </summary>
    public void OpeningMove()
    {
        transform.Translate(0, 0, 0.5f);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void Move()
    {
        MoveInput(Speed, Acceleration, Deceleration, TurnSpeed);
        PlayMovingSound(Sounds[(int)SoundsEnum.Moving]);
    }

    private float nextExec = 0.0f;
    private void MoveInput(float speed, float acceleration, float deceleration, float turnSpeed)
    {
        if (Input.GetButtonUp("Fire1"))
        {
            GainSpeed(speed, acceleration);
        }
        else
        {
            Utility.ExecuteAfter(LoseSpeedWrapper, ref nextExec, DecelerationCooldown);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Turn(turnSpeed);
        } 
    }

    /// <summary>
    /// Simple wrapper function used in order to call LoseSpeed with Utility.ExecuteAfter 
    /// </summary>
    private void LoseSpeedWrapper()
    {
        LoseSpeed(Speed, Deceleration);
    }

    /// <summary>
    /// Adds speed to the player. The faster the player moves 
    /// the faster he accelerates and vice versa
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="acceleration"></param>
    private void GainSpeed(float speed, float acceleration)
    {
        if (!isDead)
        {
            // Add speed only if below the speed limit
            if (!SpeedLimit(speed, acceleration, MaxSpeed))
                speed += acceleration;
           
            rb.AddForce(new Vector3(0, 0, speed * speedMultiplier), ForceMode.Force);
            Speed = speed;
        }
    }
 
    /// <summary>
    /// Lose speed based on the speed of movement.
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="deceleration"></param>
    private void LoseSpeed(float speed, float deceleration)
    {
        if (!isDead)
        {
            if (LowSpeedLimit(speed, deceleration, minSpeed))
            {
                deceleration = minDeceleration;
                Speed = minSpeed;
            }
            else
            {
                speed -= deceleration;

                Speed = speed;
            }
            Deceleration = deceleration;
        }
    }

    /// <summary>
    /// Turns the player given a turn speed
    /// </summary>
    /// <param name="turnSpeed">The speed of the turning</param>
    private void Turn(float turnSpeed)
    {
        if (!isDead)
        {
            float trans = (Input.GetAxis("Horizontal") * turnSpeed) * Time.deltaTime;
            transform.Translate(trans, 0, 0);
        }
    }

    /// <summary>
    /// Checks if the player has crossed the speed limit.
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="accAmount"></param>
    /// <param name="maxSpeed"></param>
    /// <returns></returns>
    private bool SpeedLimit(float speed, float accAmount, int maxSpeed)
    {
        if (speed + accAmount > maxSpeed)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if the player is moving above the lower speed limit.
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="decAmount"></param>
    /// <param name="minSpeed"></param>
    /// <returns></returns>
    private bool LowSpeedLimit(float speed, float decAmount, float minSpeed)
    {
        if (speed - decAmount < minSpeed)
            return transform;
        else
            return false;
    }

    bool playing = false;
    /// <summary>
    /// Plays sound during the movement of the player if he is not dead. 
    /// </summary>
    /// <param name="sound"></param>
    public void PlayMovingSound(AudioClip sound)
    {
        if (!isDead && !playing)
        {
            playing = true;
            ad.clip = sound;
            ad.Play();
        }
    }

    /// <summary>
    /// Plays sound when the player dies.
    /// </summary>
    /// <param name="sound"></param>
    public void PlayDeathSound(AudioClip sound)
    {
        if (isDead)
        {
            ad.clip = sound;
            ad.Play();
        }
    }

    /// <summary>
    /// Resets player in original position
    /// </summary>
    public void Reset()
    {
        if (StartGame)
        {
            isDead = false;
            Speed = 0;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            transform.position = new Vector3(0, 1, 0);
            gameObject.GetComponent<roadGenerator>().resetAccordingToPlayer();
            gameObject.GetComponent<roadGenerator>().TimesReseted = 0;
        }
    }
}
