using UnityEngine;

public class TonyPlayerControlTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 5.0f;
    public float jumpSpeed = 4.0f;
   
    private Rigidbody rb;
    private bool isGrounded;
    //Boolean to check if the player was jumping before last frame.
    private bool wasGrounded;

    //Particle Effect
    public ParticleSystem jumpParticle;
    public Transform jumpPoint;

    //Sound Effect
    public AudioClip jumpClip;
    public AudioClip landClip;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();

        //characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump();
        }

        //Check For Landing
        //If in the last frame, player was jumping, wasGrounded will be false. In this case, isGrounded will be true because the checkGrounded will make it true after It landed on the ground
        if(!wasGrounded && isGrounded)
        {
            land();
        }

        move();

        wasGrounded = isGrounded;
        
    }
    //Movement Function
    private void move()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");
        Vector3 move= transform.right * horizontalValue + transform.forward * verticalValue;
        Vector3 velocity=move*moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        // characterController.Move(move * moveSpeed * Time.deltaTime);

    }
    //Jump Function
    private void jump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
        playJumpParticle();
    }
    private void land()
    {

        if (landClip != null)
        {
            audioSource.PlayOneShot(landClip);
        }
        if (jumpParticle != null && jumpPoint != null)
        {
            Vector3 newPoint = jumpPoint.position;
            newPoint.y = newPoint.y - 0.2f;
            ParticleSystem particle = Instantiate(jumpParticle, newPoint, jumpPoint.rotation);
            particle.Play();
            Debug.Log("Particle is playing: " + jumpParticle.isPlaying);
            Destroy(particle.gameObject, 0.8f);
        }
    }
    private void playJumpParticle()
    {
        if (jumpParticle != null && jumpPoint != null)
        {
            Vector3 newPoint = jumpPoint.position;

            ParticleSystem particle = Instantiate(jumpParticle, newPoint, jumpPoint.rotation);
            particle.Play();
            Debug.Log("Particle is playing: " + jumpParticle.isPlaying);
            Destroy(particle.gameObject, 0.8f);
        }
    }
    private void checkGrounded()
    {
        isGrounded= Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
