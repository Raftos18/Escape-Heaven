using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public bool StartGame;
    public AudioClip[] Sounds;
    
    [HideInInspector] public Movement movement;
    [HideInInspector] public Death death;
    [HideInInspector] public Jump jump;

    // Put a reference to the corridor generator for easy access
    public CorridorGenerator corridorGenerator;

    public Rigidbody rigbody;
    public AudioSource audioSource;

    void Awake()
    {
        rigbody = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();

        death = GetComponent<Death>();
        corridorGenerator = GetComponent<CorridorGenerator>();
        movement = GetComponent<Movement>();
        jump = GetComponent<Jump>();        
    }

    void Start ()
    {
        StartGame = false;
        death.IsDead = false;
    }
	
    void FixedUpdate()
    {
        if (StartGame)
        {
            Debug.Log("Not Else");
            movement.MoveHandler(this);
            jump.JumpHandler(this);
        }
        else
        {
            Debug.Log("Else");
            movement.OpeningMove();
        }
    }

	void Update ()
    {
        corridorGenerator.ResetCorridorOnPoint(movement);
	}

    /// <summary>
    /// Resets player in original state.
    /// The order call matters be careful.
    /// It should be a lot simpler with calls to 
    /// different components
    /// </summary>
    public void Reset()
    {
        if (StartGame)
        {
            death.IsDead = false;
            movement.Speed = 0;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            transform.position = new Vector3(0, 3.5f, 0);
            gameObject.GetComponent<CorridorGenerator>().ResetDifficulty();
            GetComponent<CorridorGenerator>().ResetAccordingToPlayer();
            gameObject.GetComponent<CorridorGenerator>().TimesReseted = 0;
        }
    }

    /// <summary>
    /// Possible mistake here
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return death.IsDead ? true : false;
    }
}
