using UnityEngine;
using System;

/// <summary>
/// Controls the movement of the player character
/// Notes: The class has become (kinda) a dump class. The hunter class is now broken due to changes to 
/// the movement
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float TurnSpeed;
    public int MaxSpeed;
    public float MinSpeed = 0;
    public float Acceleration;
    
    public float DecelerationCooldown;

    [Header("Floating")]
    public float Amplitude = 0.5f;
    public float FloatSpeed = 0.5f;
    public float floatOffset = 1.5f;

    // These have no place here
    [Space]
    public bool StartGame;
    public AudioClip[] Sounds;
    public bool isDead;

    private Rigidbody rb;
    private AudioSource ad;
   
	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ad = gameObject.GetComponent<AudioSource>();
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
        GetComponent<CorridorGenerator>().ResetCorridorOnPoint(this);
    }

    /// <summary>
    /// Simple straight movement for the opening of the game
    /// </summary>
    public void OpeningMove()
    {
        transform.Translate(0, 0, 0.5f);
    }

    #region Move Code

    public void Move()
    {
        if(!isDead)
            MoveInput();
    }

    private float nextExec = 0.0f;
    private void MoveInput()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Accelerate(ref Speed, Acceleration);
            rb.AddForce(Vector3.forward * Speed, ForceMode.Impulse);
        }
        else
        {
             Utility.ExecuteAfter(WrapperDec, ref nextExec, DecelerationCooldown);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Turn(TurnSpeed);
        }
    }

    /// <summary>
    /// Add acceleration to speed
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="acceleration"></param>
    private void Accelerate(ref float speed, float acceleration)
    {
        if (speed < MaxSpeed)
            speed += acceleration;
    }

    /// <summary>
    /// Remove acceleration from speed
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="acceleration"></param>
    private void Deccelerate(ref float speed, float acceleration)
    {
        if (speed > MinSpeed)
            speed -= acceleration;
    }

    /// <summary>
    /// Simple wrapper func to use with Utility.ExecuteAfter 
    /// </summary>
    private void WrapperDec()
    {
        Deccelerate(ref Speed, Acceleration);
    }

    /// <summary>
    /// Turns the player with the given speed
    /// </summary>
    /// <param name="turnSpeed">The speed of the turning</param>
    private void Turn(float turnSpeed)
    {        
        float trans = (Input.GetAxis("Horizontal") * turnSpeed) * Time.deltaTime;
        transform.Translate(trans, 0, 0);
    }
    #endregion

    #region Legacy Move Code

    // private float minDeceleration;                                   // Legacy Code 
    // private const float speedMultiplier = 10.0f;                        // Simple Multiplier to keep numbers smaller. Legacy Code
    //public float Deceleration;                // Legacy code

    //public void Move()
    //{
    //    MoveInput(Speed, Acceleration, Deceleration, TurnSpeed);
    //    PlayMovingSound(Sounds[(int)SoundsEnum.Moving]);
    //}

    //private float nextExec = 0.0f;
    //private void MoveInput(float speed, float acceleration, float deceleration, float turnSpeed)
    //{
    //    if (Input.GetButtonUp("Fire1"))
    //    {
    //        GainSpeed(speed, acceleration);
    //    }
    //    else
    //    {
    //        Utility.ExecuteAfter(LoseSpeedWrapper, ref nextExec, DecelerationCooldown);
    //    }

    //    if (Input.GetAxis("Horizontal") != 0)
    //    {
    //        Turn(turnSpeed);
    //    } 
    //}

    ///// <summary>
    ///// Simple wrapper function used in order to call LoseSpeed with Utility.ExecuteAfter 
    ///// </summary>
    //private void LoseSpeedWrapper()
    //{
    //    LoseSpeed(Speed, Deceleration);
    //}

    ///// <summary>
    ///// Adds speed to the player. The faster the player moves 
    ///// the faster he accelerates and vice versa
    ///// </summary>
    ///// <param name="speed"></param>
    ///// <param name="acceleration"></param>
    //private void GainSpeed(float speed, float acceleration)
    //{
    //    if (!isDead)
    //    {
    //        // Add speed only if below the speed limit
    //        if (!SpeedLimit(speed, acceleration, MaxSpeed))
    //            speed += acceleration;

    //        rb.AddForce(new Vector3(0, 0, speed * speedMultiplier), ForceMode.Force);
    //        Speed = speed;
    //    }
    //}

    ///// <summary>
    ///// Lose speed based on the speed of movement.
    ///// </summary>
    ///// <param name="speed"></param>
    ///// <param name="deceleration"></param>
    //private void LoseSpeed(float speed, float deceleration)
    //{
    //    if (!isDead)
    //    {
    //        if (LowSpeedLimit(speed, deceleration, minSpeed))
    //        {
    //            deceleration = minDeceleration;
    //            Speed = minSpeed;
    //        }
    //        else
    //        {
    //            speed -= deceleration;

    //            Speed = speed;
    //        }
    //        Deceleration = deceleration;
    //    }
    //}

    ///// <summary>
    ///// Turns the player given a turn speed
    ///// </summary>
    ///// <param name="turnSpeed">The speed of the turning</param>
    //private void Turn(float turnSpeed)
    //{
    //    if (!isDead)
    //    {
    //        float trans = (Input.GetAxis("Horizontal") * turnSpeed) * Time.deltaTime;
    //        transform.Translate(trans, 0, 0);
    //    }
    //}

    ///// <summary>
    ///// Checks if the player has crossed the speed limit.
    ///// </summary>
    ///// <param name="speed"></param>
    ///// <param name="accAmount"></param>
    ///// <param name="maxSpeed"></param>
    ///// <returns></returns>
    //private bool SpeedLimit(float speed, float accAmount, int maxSpeed)
    //{
    //    if (speed + accAmount > maxSpeed)
    //        return true;
    //    else
    //        return false;
    //}

    ///// <summary>
    ///// Checks if the player is moving above the lower speed limit.
    ///// </summary>
    ///// <param name="speed"></param>
    ///// <param name="decAmount"></param>
    ///// <param name="minSpeed"></param>
    ///// <returns></returns>
    //private bool LowSpeedLimit(float speed, float decAmount, float minSpeed)
    //{
    //    if (speed - decAmount < minSpeed)
    //        return transform;
    //    else
    //        return false;
    //}
    #endregion

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
    /// Resets player in original state
    /// </summary>
    public void Reset()
    {
        if (StartGame)
        {
            isDead = false;
            Speed = 0;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            transform.position = new Vector3(0, 1, 5);
            GetComponent<CorridorGenerator>().ResetAccordingToPlayer();
            gameObject.GetComponent<CorridorGenerator>().TimesReseted = 0;
        }
    }
}
