using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour
{
    public float TimeHeld = 0.0f;
    public float TimeForFullJump = 2.0f;
    public float MinJumpForce = 0.5f;
    public float MaxJumpForce = 2.0f;
    public float JumpingPoint = 1.0f;

    /// <summary>
    /// Check if the gameobject is on the air
    /// </summary>
    public bool OnAir
    {
        get
        {
            if (transform.position.y > JumpingPoint)
                return true;
            else
                return false;
        }
    }

    private Movement player;
    private Rigidbody rb;

    void Start()
    {
        player = gameObject.GetComponent<Movement>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyGravity(rb);
        BalancePlayer();

        if (Input.GetButtonDown("Jump"))
        {
            TimeHeld = 0f;
        }
        if (Input.GetButton("Jump"))
        {
            TimeHeld += Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            Jumping();
        }
    }

    /// <summary>
    /// Apply gravity to the target rigidbody if he is on the air
    /// </summary>
    /// <param name="rb"></param>
    private void ApplyGravity(Rigidbody rb)
    {
        if(!OnAir)
            rb.useGravity = false;
        else
            rb.useGravity = true;
    }

    /// <summary>
    /// Checks if the player can perform a jump
    /// </summary>
    /// <param name="jumpingPoint">The point that the player must to in order to jump</param>
    /// <returns></returns>
    private bool CanJump(float jumpingPoint)
    {
        if (transform.position.y <= jumpingPoint)
            return true;
        else
            return false;
    }
   
    /// <summary>
    /// Calculates the power of the jump depending on
    /// how long the jump button is pressed
    /// </summary>
    public void Jumping()
    {
        if (!player.isDead && CanJump(JumpingPoint))
        {
            float verticalJumpForce = ((MaxJumpForce - MinJumpForce) * (TimeHeld / TimeForFullJump)) + MinJumpForce;

            if (verticalJumpForce > MaxJumpForce)
                verticalJumpForce = MaxJumpForce;
            
            Vector2 resolvedJump = new Vector2(0, verticalJumpForce);
            GetComponent<Rigidbody>().AddForce(resolvedJump, ForceMode.Impulse);
            TimeHeld = 0.0f;
        }
    }

    /// <summary>
    /// Keeps the player a bit above the ground in order to 
    /// avoid collision with the floor.
    /// </summary>
    private void BalancePlayer()
    {
        float height = 0.5f;
        float forceMultiplier = 2.0f;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, height))
        {
            rb.AddForce(transform.up * (forceMultiplier / (hit.distance / 2)));
        }
    }
}
